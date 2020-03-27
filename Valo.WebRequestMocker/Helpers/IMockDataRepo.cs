using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valo.WebRequestMocker.Model;

namespace Valo.WebRequestMocker.Helpers
{
    public interface IMockDataRepo
    {
        void SaveMockData(List<MockResponseEntry> mockedData);
        List<MockResponseEntry<T>> LoadMockData<T>();
    }
}
