using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace ActorCore
{
    /// <summary>
    /// 控制Playable之间的平滑过渡
    /// </summary>
    public class AnimationTransitionPlayable : PlayableBehaviour
    {
        private Playable _playable;
        private PlayableGraph _graph;
        private AnimationMixerPlayable _mixer;

        private int _currIndex = -1;                       // 当前正在播放的动画
        private int _targetIndex = -1;                       // 当前正在过渡的下一个动画
        private float _blendDuration;
        private float _elapsedBlendTime;
        private float _blendStart;
        private float _curStartWeight;


        public override void OnPlayableCreate(Playable playable)
        {
            _playable = playable;
            _playable.SetInputCount(1);
            _playable.SetInputWeight(0, 1);

            _graph = _playable.GetGraph();

            _mixer = AnimationMixerPlayable.Create(_graph, 0);
            _graph.Connect(_mixer, 0, _playable, 0);
        }

        public override void OnPlayableDestroy(Playable playable)
        {
            if (_graph.IsValid())
                _graph.DestroySubgraph(playable);
        }

        public int AddClipPlayable(AnimationClipPlayable playable)
        {
            if (_mixer.IsNull() || !_mixer.IsValid())
            {
                Debug.LogError("_mixer.IsNull() || !_mixer.IsValid()");
                return -1;
            }

#if UNITY_EDITOR
            for (int i = 0; i < _mixer.GetInputCount(); ++i)
            {
                AnimationClipPlayable clipPlayable = (AnimationClipPlayable)_mixer.GetInput(i);
                if (clipPlayable.IsNull() || !clipPlayable.IsValid())
                    continue;

                if (clipPlayable.GetAnimationClip().Equals(playable.GetAnimationClip()))
                {
                    Debug.LogWarning("repeated animation clip playable");
                    return i;
                }
            }
#endif
            playable.SetDuration(playable.GetAnimationClip().isLooping ? Mathf.Infinity : playable.GetAnimationClip().length);
            return _mixer.AddInput(playable, 0, 0);
        }

        public void SetTime(double v)
        {
            Playable p = GetCurrentPlayable();
            if (!p.IsValid()) return;
            p.SetTime(v);
            if (_currIndex == -1) return;

            if (_currIndex != _targetIndex)
            {
                _mixer.SetInputWeight(_currIndex, 0);
                _mixer.SetInputWeight(_targetIndex, 1);
                _currIndex = _targetIndex;
            }
        }

        public void SetSpeed(float speed)
        {
            _mixer.SetSpeed(speed);
        }

        public void PlayClip(int targetIndex, float blendTime = 0)
        {
            if (_mixer.IsNull() || !_mixer.IsValid())
            {
                Debug.LogError("_mixer.IsNull() || !_mixer.IsValid()");
                return;
            }

            int pendingTargetIndex = FindValidIndex(targetIndex);
            if (pendingTargetIndex == -1)
                return;

            // 目标clip为非循环时重新播放
            AnimationClipPlayable clipPlayable = (AnimationClipPlayable)GetPlayable(pendingTargetIndex);
            if (clipPlayable.IsValid() && clipPlayable.GetAnimationClip() != null && !clipPlayable.GetAnimationClip().isLooping)
            {
                clipPlayable.SetTime(0);
                clipPlayable.SetDone(false);        // 重置Done状态
            }

            // 设置初始数据
            _blendDuration = Mathf.Max(0, blendTime) / (float)_playable.GetSpeed();
            _blendStart = Time.time;
            _elapsedBlendTime = 0;

            if (_currIndex == _targetIndex)
            {
                if (_currIndex == -1)
                { // 初始化
                    _currIndex = pendingTargetIndex;
                    _targetIndex = pendingTargetIndex;

                    _mixer.SetInputWeight(_currIndex, 1);
                }
                else
                { // 非过渡状态下切换至pendingTargetIndex
                    _targetIndex = pendingTargetIndex;

                    _curStartWeight = 1.0f;
                }
            }
            else
            {
                if (pendingTargetIndex == _currIndex)
                { // 过渡状态下切换至_currIndex
                    int temp = _targetIndex;
                    _targetIndex = _currIndex;
                    _currIndex = temp;

                    _curStartWeight = _mixer.GetInputWeight(_currIndex);
                }
                else
                { // 过渡状态下切换至其他
                    _targetIndex = pendingTargetIndex;

                    _curStartWeight = _mixer.GetInputWeight(_currIndex);
                }
            }

            // 设置Playable
            for (int i = 0; i < _mixer.GetInputCount(); ++i)
            {
                if (i == _currIndex || i == _targetIndex)
                    continue;

                // 清空其他playable weight
                _mixer.SetInputWeight(i, 0);
            }

        }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            if (_currIndex == -1)
                return;

            if (_currIndex != _targetIndex)
            { // 立即过渡          
                if (_elapsedBlendTime <= _blendDuration - 0.0001f)
                {
                    _elapsedBlendTime = Time.time - _blendStart;
                    float weight = Mathf.Clamp01(_elapsedBlendTime / _blendDuration);

                    float curWeight = _curStartWeight * (1.0f - weight);
                    _mixer.SetInputWeight(_currIndex, curWeight);
                    _mixer.SetInputWeight(_targetIndex, 1.0f - curWeight);
                }
                else
                {
                    _mixer.SetInputWeight(_currIndex, 0);
                    _mixer.SetInputWeight(_targetIndex, 1);

                    // pause
                    //Playable curPlayable = _mixer.GetInput(_currIndex);
                    //curPlayable.Pause();

                    // 过渡结束
                    _currIndex = _targetIndex;
                }
            }
        }

        public Playable GetCurrentPlayable()
        {
            if (_targetIndex >= 0 && _targetIndex < _mixer.GetInputCount())
            {
                return _mixer.GetInput(_targetIndex);
            }
            return Playable.Null;
        }

        private Playable GetPlayable(int pendingTargetIndex)
        {
            if (pendingTargetIndex >= 0 && pendingTargetIndex < _mixer.GetInputCount())
            {
                return _mixer.GetInput(pendingTargetIndex);
            }
            return Playable.Null;
        }

        // 获取有效playable index
        private int FindValidIndex(int index)
        {
            if (index < 0 || index >= _mixer.GetInputCount())
                return -1;

            Playable playable = _mixer.GetInput(index);
            if (!playable.IsNull() && playable.IsValid())
                return index;

            return FindValidIndex(index + 1);
        }
    }
}