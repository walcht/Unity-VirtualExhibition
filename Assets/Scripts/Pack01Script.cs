using Common;
using UnityEngine;

public class Pack01Script : MonoBehaviour, IPackStands
{
    [SerializeField]
    private GameObject _stand00;

    [SerializeField]
    private GameObject _stand01;

    [SerializeField]
    private GameObject _stand02;

    [SerializeField]
    private GameObject _stand03;

    public StandInfo GetChildStand(int number)
    {
        switch (number)
        {
            case 0:
                if (transform.rotation.eulerAngles.y == 180)
                    return new StandInfo { Type = StandType.LL, ReferencedStand = _stand01 };
                return new StandInfo { Type = StandType.LR, ReferencedStand = _stand00 };

            case 1:
                if (transform.rotation.eulerAngles.y == 180)
                    return new StandInfo { Type = StandType.LR, ReferencedStand = _stand00 };
                return new StandInfo { Type = StandType.LL, ReferencedStand = _stand01 };

            case 2:
                if (transform.rotation.eulerAngles.y == 180)
                    return new StandInfo { Type = StandType.LL, ReferencedStand = _stand03 };
                return new StandInfo { Type = StandType.LR, ReferencedStand = _stand02 };

            case 3:
                if (transform.rotation.eulerAngles.y == 180)
                    return new StandInfo { Type = StandType.LR, ReferencedStand = _stand02 };
                return new StandInfo { Type = StandType.LL, ReferencedStand = _stand03 };

            default:
                return null;
        }
    }
}
