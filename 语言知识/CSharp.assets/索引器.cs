namespace 索引器
{
  
    class People
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public People[] frineds;
        
        //声明索引器   --->让对象可以像数组一样通过索引访问其中元素   
        public People this[int index]
        {
            //get set 中可以写逻辑
            get
            {
                if(frineds == null)
                {
                    return null;
                }
                else if(frineds.Length -1 < index)
                {
                    Console.WriteLine("越界");
                    return null;
                }
                return frineds[index];
            }
            set
            {
                if (frineds == null)
                {
                    frineds = new People[] { value };
                }
                else if (frineds.Length - 1 < index)
                {
                    Console.WriteLine("越界");
                    return;
                }
                //value代表插入的值
                frineds[index] = value;
            }
        }

        #region 知识点五 索引器的重载
        int[,] array;
        public int this[int x, int y]
        {
            get
            {
                return array[x, y];
            }
            set
            {
                if (array == null)
                {
                    array = new int[x+1, y+1];
                }
                array[x, y] = value;
            }
        }
        #endregion

    }

    
    internal class Program
    {
        static void Main(string[] args)
        {
            People people = new People();
            people[0] = new People();
            Console.WriteLine(people[0]);
            people[1, 2] = 5;
            Console.WriteLine(people[1,2]);
        }
    }
}

