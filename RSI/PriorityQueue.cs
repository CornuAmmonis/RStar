using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSI
{
    class PriorityQueue
    {
        public const bool SORT_ORDER_ASCENDING = true;
        public const bool SORT_ORDER_DESCENDING = false;
        public const int DEFAULT_CAPACITY = 10;

        private List<int> values = null;
        private List<float> priorities = null;
        private bool sortOrder = SORT_ORDER_ASCENDING;
        private bool useInternalConsistencyChecking;

        private Logger log;

        public PriorityQueue(Logger log, bool sortOrder) : this(log, sortOrder, DEFAULT_CAPACITY) {}

        public PriorityQueue(Logger log, bool sortOrder, int initialCapacity) 
        {
            this.log = log;
            this.sortOrder = sortOrder;
            values = new List<int>(initialCapacity);
            priorities = new List<float>(initialCapacity);
            useInternalConsistencyChecking = RSI.Properties.Settings.Default.InternalConsistencyChecking;
        }


        /// <returns>true if p1 has an earlier sort order than p2.</returns>
        private bool SortsEarlierThan(float p1, float p2) 
        {
            if (sortOrder == SORT_ORDER_ASCENDING) {
                return p1 < p2;
            }
            return p2 < p1;
        }

        // to insert a value, append it to the arrays, then
        // reheapify by promoting it to the correct place.
        public void Insert(int value, float priority) {
            values.Add(value);
            priorities.Add(priority);

            Promote(values.Count - 1, value, priority);
        }

        private void Promote(int index, int value, float priority) {
            // Consider the index to be a "hole"; i.e. don't swap priorities/values
            // when moving up the tree, simply copy the parent into the hole and
            // then consider the parent to be the hole.
            // Finally, copy the value/priority into the hole.
            while (index > 0) {
                int parentIndex = (index - 1) / 2;
                float parentPriority = priorities[parentIndex];

                if (SortsEarlierThan(parentPriority, priority)) {
                    break;
                }

                // copy the parent entry into the current index.
                values[index] = values[parentIndex];
                priorities[index] = parentPriority;
                index = parentIndex;
            }

            values[index] = value;
            priorities[index] = priority;

            if (UseInternalConsistencyChecking()) {
                Check();
            }
        }

        public int Size() {
            return values.Count;
        }

        public void Clear() {
            values.Clear();
            values.Capacity = DEFAULT_CAPACITY;
            priorities.Clear();
            priorities.Capacity = DEFAULT_CAPACITY;
        }

        // List Clear() is O(n), maintains original capacity like Trove TIntArrayList
        // Would be better to implement an O(1) Reset.
        public void Reset() {
            values.Clear();
            priorities.Clear();
        }

        public int GetValue() {
            return values[0];
        }

        public float GetPriority() {
            return priorities[0];
        }

        private void Demote(int index, int value, float priority) {
            int childIndex = (index * 2) + 1; // left child

            while (childIndex < values.Count) {
                float childPriority = priorities[childIndex];

                if (childIndex + 1 < values.Count) {
                    float rightPriority = priorities[childIndex + 1];
                    if (SortsEarlierThan(rightPriority, childPriority)) {
                      childPriority = rightPriority;
                      childIndex++; // right child
                    }
                }

                if (SortsEarlierThan(childPriority, priority)) {
                    priorities[index] = childPriority;
                    values[index] = values[childIndex];
                    index = childIndex;
                    childIndex = (index * 2) + 1;
                } else {
                    break;
                }
            }

            values[index] = value;
            priorities[index] = priority;
        }

        // get the value with the lowest priority
        // creates a "hole" at the root of the tree.
        // The algorithm swaps the hole with the appropriate child, until
        // the last entry will fit correctly into the hole (ie is lower
        // priority than its children)
        public int Pop() {
            int ret = values[0];

            // record the value/priority of the last entry
            int lastIndex = values.Count - 1;
            int tempValue = values[lastIndex];
            float tempPriority = priorities[lastIndex];

            values.RemoveAt(lastIndex);
            priorities.RemoveAt(lastIndex);

            if (lastIndex > 0) {
                Demote(0, tempValue, tempPriority);
            }

            if (UseInternalConsistencyChecking()) {
                Check();
            }

            return ret;
        }

        public void SetSortOrder(bool sortOrder) {
            if (this.sortOrder != sortOrder) {
                this.sortOrder = sortOrder;
                // reheapify the arrays
                for (int i = (values.Count / 2) - 1; i >= 0; i--) {
                    Demote(i, values[i], priorities[i]);
                }
            }
            if (UseInternalConsistencyChecking()) {
                Check();
            }
        }

        private void Check() {
            // for each entry, check that the child entries have a lower or equal
            // priority
            int lastIndex = values.Count - 1;

            for (int i = 0; i < values.Count / 2; i++) {
                float currentPriority = priorities[i];

                int leftIndex = (i * 2) + 1;
                if (leftIndex <= lastIndex) {
                float leftPriority = priorities[leftIndex];
                    if (SortsEarlierThan(leftPriority, currentPriority)) {
                        log.Error("Internal error in PriorityQueue");
                    }
                }

                int rightIndex = (i * 2) + 2;
                if (rightIndex <= lastIndex) {
                    float rightPriority = priorities[rightIndex];
                    if (SortsEarlierThan(rightPriority, currentPriority)) {
                        log.Error("Internal error in PriorityQueue");
                    }
                }
            }
        }

        // internal consistency checking - set to true if debugging tree corruption
        private bool UseInternalConsistencyChecking()
        {
            return useInternalConsistencyChecking;
        }
    }
}
