public class DataManagers {

    private static DataManagers sInstance;

    public DataManagerBase<ObjData, ObjDataList> ObjDataManager;
    public DataManagerBase<ItemData, ItemDataList> ItemDataManager;
    public DataManagerBase<MaterialData, MaterialDataList> MaterialDataManager;
    public DataManagerBase<AnimalData, AnimalDataList> AnimalDataManager;
    public DataManagerBase<PlantData, PlantDataList> PlantDataManager;
    public DataManagerBase<BuildingData, BuildingDataList> BuildingDataManager;

    public static DataManagers Instance{
        get
        {
            if(null == sInstance)
            {
                sInstance = new DataManagers();
            }
            return sInstance;
        }
    }

    private DataManagers()
    {
        initManagers();
    }

    private void initManagers()
    {
        ObjDataManager = DataManagerBase<ObjData, ObjDataList>.Instance;
        ItemDataManager = DataManagerBase<ItemData, ItemDataList>.Instance;
        MaterialDataManager = DataManagerBase<MaterialData, MaterialDataList>.Instance;
        AnimalDataManager = DataManagerBase<AnimalData, AnimalDataList>.Instance;
        PlantDataManager = DataManagerBase<PlantData, PlantDataList>.Instance;
        BuildingDataManager = DataManagerBase<BuildingData, BuildingDataList>.Instance;
        //ObjDataManager.LoadData("");
    }
}
