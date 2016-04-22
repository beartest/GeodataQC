using System;
using System.IO;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using System.Data;
using System.Windows.Forms;

namespace myCarto
{
    public class FeatureDataFromPFW
    {
        IFeatureWorkspace pFWorkspace;
        ILayer pLayer;
        ITable pTable;
        DataTable dTable;
        DataTable wrongTable;

        public FeatureDataFromPFW(string pFWpath)
        {
            string pathtype = Path.GetExtension(pFWpath).ToLower();
            pFWorkFromGDB pfwFile = new pFWorkFromGDB(pFWpath);
            if (pathtype == ".mdb")
                this.pFWorkspace = pfwFile.GetPFWorkspace();
            else if (pathtype == ".shp")
                this.pFWorkspace = pfwFile.GetShpWorkspace();
        }

        public FeatureDataFromPFW(string pFWpath, string Filename)
        {
            string pathtype = Path.GetExtension(pFWpath).ToLower();
            pFWorkFromGDB pfwFile = new pFWorkFromGDB(pFWpath);
            if (pathtype == ".mdb")
                this.pFWorkspace = pfwFile.GetPFWorkspace();
            else if (pathtype == ".shp")
                this.pFWorkspace = pfwFile.GetShpWorkspace();
            FDLayerFromPFW(Filename);
            FDTableFromPFW(Filename);
        }

        public void FDLayerFromPFW(string Filename)
        {
            IFeatureLayer pFeatureLayer = new FeatureLayerClass();
            pFeatureLayer.FeatureClass = pFWorkspace.OpenFeatureClass(Filename);
            pFeatureLayer.Name = Filename;
            pLayer = pFeatureLayer as ILayer;
        }

        public void FDTableFromPFW(string Filename)
        {
            pTable = pFWorkspace.OpenTable(Filename);
            dTable = ToDataTable(pTable);
            wrongTable = SetWrongTable();
        }
        private DataTable SetWrongTable()
        {
            try
            {
                /*if (pTable == null)
                {
                    FDTableFromPFW(Filename);
                }
                if (dTable == null)
                {
                    dTable = ToDataTable(pTable);
                }*/
                DataTable wt = new DataTable();
                for (int i = 0; i < dTable.Columns.Count; i++)
                {
                    wt.Columns.Add(dTable.Columns[i].Caption);
                }
                //wt.Rows.Add(dTable.Rows[0].ItemArray);
                for (int i = 0; i < dTable.Rows.Count; i++)
                {
                    bool ok = false;
                    for (int j = 0; j < dTable.Columns.Count; j++)
                    {
                        if (dTable.Rows[i][j].ToString().Trim() == "")
                        {
                            ok = true;
                            break;
                        }
                    }
                    if (ok == true)
                    {
                        wt.Rows.Add(dTable.Rows[i].ItemArray);
                    }
                }
                return wt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public ILayer GetPLayer
        {
            get
            {
                return pLayer;
            }
        }
        public ITable GetPTable
        {
            get 
            {
                return pTable;
            }
        }
        public DataTable GetdTable
        {
            get
            {
                return dTable;
            }
        }
        public DataTable GetwTable
        {
            get
            {
                return wrongTable;
            }
        }
        public static DataTable ToDataTable(ITable mTable)
        {
            try
            {
                DataTable pTable = new DataTable();
                for (int i = 0; i < mTable.Fields.FieldCount; i++)
                {
                    pTable.Columns.Add(mTable.Fields.get_Field(i).Name);
                }

                ICursor pCursor = mTable.Search(null, false);
                IRow pRrow = pCursor.NextRow();
                while (pRrow != null)
                {
                    DataRow pRow = pTable.NewRow();
                    string[] StrRow = new string[pRrow.Fields.FieldCount];
                    for (int i = 0; i < pRrow.Fields.FieldCount; i++)
                    {
                        StrRow[i] = pRrow.get_Value(i).ToString();
                    }
                    pRow.ItemArray = StrRow;
                    pTable.Rows.Add(pRow);
                    pRrow = pCursor.NextRow();
                }

                return pTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
    }
}
