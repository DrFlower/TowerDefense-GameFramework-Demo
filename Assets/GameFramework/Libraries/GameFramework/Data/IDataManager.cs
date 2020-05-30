using System.Collections.Generic;

namespace GameFramework.Data
{
    public interface IDataManager
    {
        int DataCount { get; }

        T GetData<T>() where T : Data;

        Data GetData(string name);

        bool HasData<T>() where T : Data;

        bool HasData(string name);

        Data[] GetAllData();

        void GetAllData(List<Data> result);

        void AddData<T>() where T : Data;

        void AddData(Data Data);

        void RemoveData<T>() where T : Data;

        void RemoveData(Data Data);

        void InitAllData();

        void PreLoadAllData();

        void LoadAllData();

        void UnLoadAllData();

    }
}


