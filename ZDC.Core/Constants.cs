using System.Collections.Generic;

namespace ZDC.Core
{
    public class Constants
    {
        public static List<string> ControllerPositions =>
            new()
            {
                /* CENTER */
                "DC_C", "DC_0", "DC_1", "DC_2", "DC_3", "DC_5", "DC_N", "DC_S", "DC_E", "DC_W", "DC_I",
                /* BRAVO */
                "DCA_", "IAD_", "BWI_", "PCT_", "ADW_",
                /* CHARLIE */
                "RIC_", "ROA_", "ORF_", "ACY_", "NGU_", "NTU_", "NHK_", "RDU_",
                /* DELTA */
                "CHO_", "HGR_", "LYH_", "EWN_", "LWB_", "ISO_", "MTN_", "HEF_", "MRB_", "PHF_", "SBY_",
                "NUI_", "FAY_", "ILM_", "NKT_", "NCA_", "NYG_", "DAA_", "DOV_", "POB_", "GSB_", "WAL_", "CVN_"
            };
    }
}