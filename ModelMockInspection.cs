using FeatureInspection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature_Inspection.UnitTests
{
    class ModelMockInspection 
    {

        private readonly string connection_string = "DSN=unipointDB;UID=jbread;PWD=Cloudy2Day";

        public ModelMockInspection()
        {

        }

        public DataTable AdapterUpdateInspection(DataTable table)
        {
            throw new NotImplementedException();
        }

        public void CreateInspectionInInspectionTable(int opkey)
        {
            throw new NotImplementedException();
        }

        public DataTable GetChartData(int opKey, int featureKey)
        {
            throw new NotImplementedException();
        }

        public DataTable GetFeatureList(int opkey)
        {
            throw new NotImplementedException();
        }

        public DataTable GetFeaturesOnOpKey(int opkey)
        {
            throw new NotImplementedException();
        }

        public DataTable GetFeaturesOnPartIndex(int partIndex, int opKey)
        {
            DataTable t;

            using (OdbcConnection conn = new OdbcConnection(connection_string))
            using (OdbcCommand com = conn.CreateCommand())
            using (OdbcDataAdapter dataAdapter = new OdbcDataAdapter(com))
            {
                string query = "SELECT Features.Sketch_Bubble AS 'Sketch Bubble', CAST(Features.FeatureType AS varchar(50)) + ' ' + CAST(Features.Nominal AS varchar(50)) + ' +' + CAST(Features.Plus_Tolerance AS varchar(50)) + ' -' + CAST(Features.Minus_Tolerance AS varchar(50)) AS Feature, "
                    + "Position.Inspection_Key_FK, Features.Feature_Key, Position.Position_Key, Measured_Value AS 'Measured Actual', Position.Old_Value AS 'Old Value', Position.Oldest_Value AS 'Oldest Value', Position.InspectionTool, Position.OutOfTolerance FROM ATI_FeatureInspection.dbo.Position " +
                                " LEFT JOIN ATI_FeatureInspection.dbo.Features ON Position.Feature_Key = Features.Feature_Key" +
                                " WHERE Inspection_Key_FK = (SELECT Inspection_Key FROM ATI_FeatureInspection.dbo.Inspection" +
                                " WHERE Op_Key = " + opKey + ") AND Piece_ID = " + partIndex + ";";

                com.CommandText = query;
                t = new DataTable();
                dataAdapter.Fill(t);

            }

            return t;
        }

        public DataTable GetInfoFromOpKeyEntry(int opkey)
        {
            throw new NotImplementedException();
        }

        public bool GetInspectionExistsOnOpKey(int opkey)
        {
            throw new NotImplementedException();
        }

        public string GetLotSize(int opkey)
        {
            throw new NotImplementedException();
        }

        public DataTable GetPartsList(int opkey)
        {
            throw new NotImplementedException();
        }

        public void InsertLotSizeToInspectionTable(int lotSize, int opkey)
        {
            throw new NotImplementedException();
        }

        public void InsertPartsToPositionTable(int opkey, int lotSize)
        {
            throw new NotImplementedException();
        }
    }
}
