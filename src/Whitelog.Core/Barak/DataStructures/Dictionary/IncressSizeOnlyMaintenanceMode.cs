using System;

namespace Whitelog.Barak.Common.DataStructures.Dictionary
{
    public class IncressSizeOnlyMaintenanceMode : IMaintenanceMode
    {
        private int m_minSize;
        private double m_maxSeedRatio;
        private int m_changefactor;

        public IncressSizeOnlyMaintenanceMode(int minSize,double maxSeedRatio,int changefactor)
        {
            m_changefactor = changefactor;
            m_maxSeedRatio = maxSeedRatio;
            m_minSize = minSize;
        }

        public int ExpectedSize(int nodes, int seeds, int maxduplicatehashcount)
        {
            if (seeds  == 0)
            {
                return Math.Max(nodes, m_minSize);
            }
            double ratio = seeds/(double)nodes;
            if (ratio >= m_maxSeedRatio)
            {
                int expectedSize = Math.Max(m_minSize, nodes*m_changefactor);
                while (seeds / expectedSize > m_maxSeedRatio)
                {
                    expectedSize = expectedSize*m_changefactor;
                }
                return expectedSize;
            }

            return nodes;
        }
    }
}