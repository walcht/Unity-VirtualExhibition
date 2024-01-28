using Common;
using UnityEngine;

public class Pack00Script : MonoBehaviour, IPackStands
{
    [SerializeField]
    private GameObject _stand00;

    [SerializeField]
    private GameObject _stand01;

    [SerializeField]
    private GameObject _stand02;

    public StandInfo GetChildStand(int number)
    {
        switch (number)
        {
            case 0:
                return new StandInfo { Type = StandType.XL, ReferencedStand = _stand00 };

            case 1:
                if (transform.rotation.eulerAngles.y == 180)
                    return new StandInfo { Type = StandType.LL, ReferencedStand = _stand02 };
                return new StandInfo { Type = StandType.LR, ReferencedStand = _stand01 };

            case 2:
                if (transform.rotation.eulerAngles.y == 180)
                    return new StandInfo { Type = StandType.LR, ReferencedStand = _stand01 };
                return new StandInfo { Type = StandType.LL, ReferencedStand = _stand02 };

            default:
                return null;
        }
    }
}
