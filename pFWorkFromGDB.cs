using System.IO;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesFile;

namespace myCarto
{
    class pFWorkFromGDB
    {
        string GDBpath;
        public pFWorkFromGDB(string pathstr)
        {
            this.GDBpath = pathstr;
        }
        public IFeatureWorkspace GetPFWorkspace()
        {
            IWorkspaceFactory pFact = new AccessWorkspaceFactoryClass();
            IWorkspace pWorkspace = null;
            //geodatabase全路径（含扩展名）
            //打开geodatabase
            pWorkspace = pFact.OpenFromFile(GDBpath, 0);

            IFeatureWorkspace pFWorkspace;
            //IGeoFeatureLayer pLayer;
            pFWorkspace = pWorkspace as IFeatureWorkspace;
            return pFWorkspace;
        }
        public IFeatureWorkspace GetShpWorkspace()
        {
            IWorkspaceFactory pFact = new ShapefileWorkspaceFactory();
            IWorkspace pWorkspace = null;
            //打开geodatabase
            
            pWorkspace = pFact.OpenFromFile(Path.GetDirectoryName(GDBpath), 0);
            IFeatureWorkspace pFWorkspace;
            //IGeoFeatureLayer pLayer;
            pFWorkspace = pWorkspace as IFeatureWorkspace;
            return pFWorkspace;
        }
        public static IFeatureWorkspace GetPFWorkspace(string GDBpath)
        {
            IWorkspaceFactory pFact = new AccessWorkspaceFactoryClass();
            IWorkspace pWorkspace = null;
            //geodatabase全路径（含扩展名）
            //打开geodatabase
            pWorkspace = pFact.OpenFromFile(GDBpath, 0);

            IFeatureWorkspace pFWorkspace;
            //IGeoFeatureLayer pLayer;
            pFWorkspace = pWorkspace as IFeatureWorkspace;
            return pFWorkspace;
        }
    }
}
