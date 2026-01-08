using UnityEngine;

public class DragHandler : MonoBehaviour
{
    private Unit unit;
    private Vector3 offset;

    void Awake()
    {
        unit = GetComponent<Unit>();
    }

    void OnMouseDown()
    {
        offset = transform.position - GetMouseWorldPos();   
        GridManager.Instance.Remove(unit.gridX, unit.gridY);
    }

    void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + offset;
    }

    void OnMouseUp()
    {
        TrySnap();
    }

    void TrySnap()  //thay đổi vị trí unit
    {
        GridManager grid = GridManager.Instance;

        int x = Mathf.RoundToInt((transform.position.x - grid.origin.x) / grid.cellSize);   //tính tọa độ (x,y) của vị trí mới
        int y = Mathf.RoundToInt((transform.position.y - grid.origin.y) / grid.cellSize);

        if (!grid.IsValid(x, y))    //nếu vị trí nằm ngoài grid thì trả về chỗ cũ
        {
            SnapBack();
            return;
        }

        Unit other = grid.GetUnit(x, y);

        if (other != null)
        {
            if (other.level == unit.level && other.type == unit.type)
            {
                TryMerge(other);
            }
            else
            {
                SwapWith(other);
            }
            return;
        }

        grid.Place(unit, x, y);     //nếu vị trí mới đang không có unit thì đặt unit vào vị trí mới
    }

    void SnapBack()     //trả unit về vị trí cũ trong trường hợp không di chuyển được
    {
        GridManager.Instance.Place(unit, unit.gridX, unit.gridY);
    }

    void TryMerge(Unit other)   //hàm merge unit, cần sửa
    {
        if (other.level != unit.level)
        {
            SnapBack();
            return;
        }

        // MERGE
        GridManager.Instance.Remove(other.gridX, other.gridY);
        Destroy(other.gameObject);

        unit.LevelUp();

        GridManager.Instance.Place(unit, other.gridX, other.gridY);
    }

    void SwapWith(Unit other)
    {
        GridManager grid = GridManager.Instance;

        // Lưu vị trí cũ của unit đang kéo
        int oldX = unit.gridX;
        int oldY = unit.gridY;

        // Vị trí của unit còn lại
        int otherX = other.gridX;
        int otherY = other.gridY;

        // Gỡ unit còn lại khỏi grid
        grid.Remove(otherX, otherY);

        // Đặt unit đang kéo vào vị trí mới
        grid.Place(unit, otherX, otherY);

        // Đặt unit còn lại về vị trí cũ
        grid.Place(other, oldX, oldY);
    }


    Vector3 GetMouseWorldPos() //lấy vị trí chuột
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //chuyển từ tọa độ screen(màn hình điện thoại) sang tọa độ world(Unity)
        pos.z = 0;
        return pos;
    }
}
