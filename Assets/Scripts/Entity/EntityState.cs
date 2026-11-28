using UnityEngine;


public interface IEntityState
{
    // Trả về dữ liệu trạng thái dưới dạng JSON
    string CaptureState();

    // Khôi phục trạng thái từ JSON
    void RestoreState(string state);
}