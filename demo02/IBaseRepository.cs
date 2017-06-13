using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo02
{
    public interface IBaseRepository<T> where T : class
    {
        void Insert(T model);
        void Delete(T model);

        void Update(T model);
        void UpdateBy(Dictionary<string, string> destFields, Dictionary<string, string> whereFields);

        T SelectBy(int Id);
        IEnumerable<T> SelectBy(Dictionary<string, string> fields);


        //分页
    }

    public interface IBaseInfoRepository<T> where T : class
    {
        T1 SelectInfoBy<T1>(int Id);
        IEnumerable<T1> SelectInfoBy<T1>(Dictionary<string, string> fields);
    }
}
