using UnityEngine;
using System.Collections;

namespace Asset
{
    public class ObjectAllocator
    {

        const int LIST_END = -1;
        const int ALLOCED = -2;
        struct Slot
        {
            public int next;
            public object obj;

            public Slot(int next, object obj)
            {
                this.next = next;
                this.obj = obj;
            }
        }

        private Slot[] list = new Slot[512];
        private int freelist = LIST_END;
        private int count = 0;

        void extend_capacity()
        {
            Slot[] new_list = new Slot[list.Length * 2];
            for (int i = 0; i < list.Length; i++)
            {
                new_list[i] = list[i];
            }
            list = new_list;
        }

        public object Get(int index)
        {
            if (index >= 0 && index < count)
            {
                return list[index].obj;
            }
            return null;
        }

        public int Add(object obj)
        {
            int index = LIST_END;

            if (freelist == LIST_END)
            {
                if (count == list.Length)
                {
                    extend_capacity();
                }

                index = count;
                list[index] = new Slot(ALLOCED, obj);
                count = index + 1;
            }
            else
            {
                index = freelist;
                list[index].obj = obj;
                freelist = list[index].next;
                list[index].next = ALLOCED;
            }

            return index;
        }

        public object Remove(int index, object o)
        {
            if (index >= 0 && index < count && list[index].next == ALLOCED)
            {
                object obj = list[index].obj;
                list[index].obj = null;
                list[index].next = freelist;
                freelist = index;
                return obj;
            }

            return null;
        }

    }
}