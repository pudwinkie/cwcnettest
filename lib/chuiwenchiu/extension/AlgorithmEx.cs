static class ArrayExtension
{
    public static void Sort<T>(this T[,] array,int d2Idx)
    {           
        int count = array.GetLength(0);
         
        T[] tempArray = new T[count];                       
        for (int idx = 0; idx < count; ++idx)
        {
            tempArray[idx] = array[idx, d2Idx];               
        }
        Array.Sort(tempArray);
 
        for (int idx = 0; idx < count; ++idx)
        {
            T tempValue = tempArray[idx];
            for (int idx2 = idx+1; idx2 < count; ++idx2)
            {
                if (array[idx2, d2Idx].Equals(tempValue))
                {
                    array.Swap(idx, d2Idx, idx2, d2Idx);
                    int d2Idx2 = (d2Idx == 0) ? 1 : 0;
                    array.Swap(idx, d2Idx2, idx2, d2Idx2);
                }
            }
        }
    }
 
    public static void Swap<T>(this T[,] array, int idx1, int idx2, int targetIdx1, int targetIdx2)
    {
        T temp;
        temp = array[targetIdx1, targetIdx2];
        array[targetIdx1, targetIdx2] = array[idx1, idx2];
        array[idx1, idx2] = temp;
    }
}