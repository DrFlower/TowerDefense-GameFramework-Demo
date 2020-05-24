using System.Collections.Generic;

namespace GameFramework.Data
{
    public interface IDataManager
    {
        int DataCount { get; }

        T GetData<T>() where T : DataBase;

        DataBase GetData(string name);

        bool HasData<T>() where T : DataBase;

        bool HasData(string name);

        DataBase[] GetAllData();

        void GetAllData(List<DataBase> result);

        void AddData<T>() where T : DataBase;

        void AddData(DataBase dataBase);

        void RemoveData(DataBase dataBase);

        void InitAllData();

        void PreLoadAllData();

        void LoadAllData();

        void UnLoadAllData();

    }
}


