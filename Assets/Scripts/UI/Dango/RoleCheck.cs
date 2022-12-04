using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleCheck
{
    public DangoColor GetReach(List<DangoColor>dangos,int current)
    {
        if (ALLRole(dangos) != DangoColor.None)
            return ALLRole(dangos);
        if (RepetitionRole(dangos, current) != DangoColor.None)
            return RepetitionRole(dangos, current);
        if (LoopRole(dangos,current)!=DangoColor.None)
            return LoopRole(dangos,current);
        if (SplitRole(dangos,current)!=DangoColor.None)
            return  SplitRole(dangos,current);
        return DangoColor.None;
    }

    private DangoColor ALLRole(List<DangoColor> dangos)
    {
        int Count=0;
        //àÍêF
        for (int i = 0; i < dangos.Count; i++)
                if (dangos[0] == dangos[i])
                {
                    Count++;
                }
        if(Count>=dangos.Count)
            return dangos[0];

        return DangoColor.None;
    }

    private DangoColor RepetitionRole(List<DangoColor> dangos, int current)
    {
        switch (current)
        {
            case 3:
                if (dangos[0] != dangos[1])
                return dangos[0];
                break;
            case 4:
                if (dangos[1] == dangos[2] && dangos[0] != dangos[1])
                    return dangos[0];
                break;
            case 5:
                if (dangos[1] == dangos[3] && dangos[0] != dangos[1])
                    return dangos[0];
                break;
            case 6:
                if (dangos[1] == dangos[2] && dangos[3] == dangos[4])
                    if (dangos[0] != dangos[1]|| dangos[0] != dangos[2])
                    return dangos[0];
                if (dangos[1] == dangos[4] && dangos[2] == dangos[3])
                    if (dangos[0] != dangos[1] || dangos[0] != dangos[2])
                        return dangos[0];
                break;
            case 7:
                if (dangos[1] == dangos[5] && dangos[2] == dangos[4])
                    if (dangos[0] != dangos[1] || dangos[0] != dangos[2] || dangos[0] != dangos[3])
                        return dangos[0];
                        break;
        }

        return DangoColor.None;
    }
    private DangoColor LoopRole(List<DangoColor> dangos, int current)
    {
        switch (current)
        {
            case 4:
                if (dangos[0] == dangos[2] && dangos[1] != dangos[0])
                    return dangos[1];
                break;
            case 6:
                if (dangos[0] == dangos[3] && dangos[1] == dangos[4])
                    if (dangos[2] != dangos[0] || dangos[2] != dangos[1])
                    return dangos[2];
                if (dangos[0] == dangos[2] && dangos[1] == dangos[3] && dangos[0] == dangos[4])
                    return dangos[1];
                break;
        }
        return DangoColor.None;
    }
    private DangoColor SplitRole(List<DangoColor> dangos,int current)
    {
        switch (current)
        {
            case 4:
                if (dangos[0] == dangos[1] && dangos[2] != dangos[0])
                    return dangos[2];
                break;
            case 6:
                if (dangos[0] == dangos[1] && dangos[1] == dangos[2] && dangos[2] != dangos[3] && dangos[3] == dangos[4])
                        return dangos[3];
                if (dangos[0] == dangos[1] && dangos[1] != dangos[2] && dangos[2] == dangos[3] && dangos[3] != dangos[4])
                    return dangos[4];
                break;
        }
        return DangoColor.None;
    }
}
