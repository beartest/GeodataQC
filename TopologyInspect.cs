using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataInspection;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;

namespace DataInspection
{
    class TopologyInspect
    {
        public  TopologyInspect()
        { }
        public bool polygoncontain(IFeatureClass SourceFeatureClass, IFeatureClass InspectedFeatureClass)
        {
            IFeature SourceFeature = SourceFeatureClass.GetFeature(1);
            IFeature InspectedFeature = InspectedFeatureClass.GetFeature(1);
            return polygoncontain(SourceFeature, InspectedFeature);
        }
        public bool polygoncontain(IFeature SourceFeature, IFeature InspectedFeature)
        {
            //iscontains==true表示SourceFeature包含InspectedFeature
            IRelationalOperator RelationalOperator = SourceFeature.Shape as IRelationalOperator;
            bool iscontains = false;
            iscontains = RelationalOperator.Contains(InspectedFeature.Shape);
            return iscontains;
        }
        public bool polinedisjust(IFeature SourceFeature, IFeature InspectedFeature)
        {
            //isdisjust==true 表示两个线要素不相交
            IRelationalOperator RelationalOperator = SourceFeature.Shape as IRelationalOperator;
            bool isdisjust = false;
            isdisjust = RelationalOperator.Disjoint(InspectedFeature.Shape);
            return isdisjust;
        }
        public List<int[]> polinedisjust(IFeatureClass SourceFeatureClass)
        {
                List<int[]> oidlist = new List<int[]>();
                ISpatialFilter SpatialFilter = new SpatialFilterClass();
                IFeatureCursor FeatureCursor = SourceFeatureClass.Search(SpatialFilter, false);
                while (true)
                {
                    IFeature Feature = FeatureCursor.NextFeature();
                    IFeatureCursor interFeatureCursor = FeatureCursor;
                    if (Feature == null)
                        break;
                    bool disjust = false;
                    while (true)
                    {
                        IFeature interfeature = interFeatureCursor.NextFeature();
                        if (interfeature == null)
                            break;
                        disjust = polinedisjust(Feature, interfeature);
                        if (!disjust)
                        {
                            int[] oids=new int[2] {Feature.OID,interfeature.OID};
                            oidlist.Add(oids);
                        }
                    }
                }
                return oidlist;
         }

        public List<int[]> polinedisjust(IFeatureClass FeatureClass, IFeatureClass OtherFeatureClass)
        {
            List<int[]> oidlist = new List<int[]>();
            ISpatialFilter SpatialFilter = new SpatialFilterClass();
            IFeatureCursor FeatureCursor = FeatureClass.Search(SpatialFilter, false);

            while (true)
            {
                IFeature Feature = FeatureCursor.NextFeature();
                if (Feature == null)
                    break;
                bool disjust = false;
                IFeatureCursor OhterFeatureCursor = OtherFeatureClass.Search(SpatialFilter, false);
                while (true)
                {
                    IFeature OhterFeature = OhterFeatureCursor.NextFeature();
                    if (OhterFeature == null)
                        break;
                    disjust = polinedisjust(Feature, OhterFeature);
                    if (!disjust)
                    {
                        int[] oids = new int[2] { Feature.OID, OhterFeature.OID };
                        oidlist.Add(oids);
                    }
                }
            }
            return oidlist;
        }
        public bool polygonOverlaps(IFeature SourceFeature, IFeature InspectedFeature)
        {
            //isOverlaps==true 表示两个面要素有重叠
            IRelationalOperator RelationalOperator = SourceFeature.Shape as IRelationalOperator;
            bool isOverlaps = false;
            isOverlaps = RelationalOperator.Overlaps(InspectedFeature.Shape);
            return isOverlaps;
        }
    }
}
