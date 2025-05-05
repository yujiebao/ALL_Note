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

索引器对于我们来说的主要作用
可以让我们以中括号的形式范围自定义类中的元素 规则自己定 访问时和数组一样
比较适用于 在类中有数组变量时使用 可以方便的访问和进行逻辑处理
固定写法
访问修饰符 返回值 this[参数列表]
get和set语句块
可以重载
注意:结构体里面也是支持索引器