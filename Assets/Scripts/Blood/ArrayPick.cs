using System;

namespace Blood {
    public class ArrayPick<T> {
        public static T GetRandomFromArray (params T[] arrayIn) {
            int randomIndex = new Random ().Next (1, arrayIn.Length + 1);
            return arrayIn[randomIndex - 1];
        }
    }
}
