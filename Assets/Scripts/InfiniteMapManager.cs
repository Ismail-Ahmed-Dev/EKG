#region Old
//using System.Collections.Generic;
//using UnityEngine;

//public class InfiniteMapManager : MonoBehaviour
//{
//    [Header("إعدادات الخريطة")]
//    public List<Transform> mapPieces;
//    public float mapWidth = 46.08f;
//    public static float currentMoveVelocity = 0f;

//    [Header("إعدادات السرعة")]
//    public float moveSpeed = 5f;
//    public float runSpeed = 10f;

//    [Header("مرجع اللاعب (اسحب اللاعب هنا)")]
//    public Transform playerTransform;

//    private float currentSpeed;
//    private float moveInput;
//    private bool isRunning;

//    void Start()
//    {
//        if (playerTransform == null)
//        {
//            playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
//            if (playerTransform == null)
//            {
//                Debug.LogError("Player Transform not found! Please assign it in Inspector.");
//            }
//        }
//    }

//    void Update()
//    {
//        // Update تبقى مسؤولة عن "قراءة" المدخلات فقط
//        moveInput = Input.GetAxis("Horizontal");
//        isRunning = Input.GetKey(KeyCode.LeftShift);
//    }

//    // FixedUpdate هو المكان "الصحيح" للتعامل مع الحركة والفيزياء
//    void FixedUpdate()
//    {
//        if (Player.isDead)
//        {
//            currentMoveVelocity = 0; // مهم عشان الـ Spawner يقف
//            return;
//        }
//        currentSpeed = isRunning ? runSpeed : moveSpeed;

//        // 1. حساب السرعة
//        float moveVelocity = -moveInput * currentSpeed;

//        // 2. "بث" السرعة الحالية للجميع (للأعمدة)
//        currentMoveVelocity = moveVelocity; // <-- الخطوة الأهم

//        // إذا لم يكن هناك إدخال، لا تفعل شيئاً
//        if (moveVelocity == 0) return;

//        // 3. تحريك كل قطع الخريطة
//        foreach (Transform piece in mapPieces)
//        {
//            piece.Translate(moveVelocity * Time.fixedDeltaTime, 0, 0);
//        }

//        // 4. التحقق من "التدوير" (الخدعة اللانهائية)
//        CheckLoop();
//    }
//    //void Update()
//    //{
//    //    moveInput = Input.GetAxis("Horizontal");
//    //    isRunning = Input.GetKey(KeyCode.LeftShift);
//    //    currentSpeed = isRunning ? runSpeed : moveSpeed;

//    //    float moveVelocity = -moveInput * currentSpeed;

//    //    if (moveVelocity == 0) return;

//    //    foreach (Transform piece in mapPieces)
//    //    {
//    //        piece.Translate(moveVelocity * Time.deltaTime, 0, 0);
//    //    }

//    //    CheckLoop();
//    //}

//    void CheckLoop()
//    {
//        // 1. موقع اللاعب (المرجع)
//        float referenceX = playerTransform.position.x;

//        // 2. العرض الإجمالي لكل القطع (مثال: قطعتين * 46.08 = 92.16)
//        float totalWidth = mapWidth * mapPieces.Count;

//        // 3. "نصف" العرض الإجمالي (مثال: 92.16 / 2 = 46.08)
//        // هذه هي المسافة "الآمنة" القصوى التي يمكن أن يبتعدها "مركز" القطعة عن اللاعب
//        float halfTotalWidth = totalWidth / 2f;

//        // 4. المرور على كل قطعة
//        foreach (Transform piece in mapPieces)
//        {
//            // 5. حساب المسافة بين "مركز" القطعة و "مركز" اللاعب
//            float distance = piece.position.x - referenceX;

//            // --- الحالة الأولى: القطعة أصبحت "يسار" اللاعب بمسافة كبيرة جداً ---
//            // (اللاعب كان يتحرك يميناً)
//            if (distance < -halfTotalWidth)
//            {
//                // انقل القطعة فوراً إلى أقصى اليمين
//                piece.position = new Vector3(piece.position.x + totalWidth, piece.position.y, piece.position.z);
//                // Debug.Log($"Moved {piece.name} RIGHT");
//            }

//            // --- الحالة الثانية: القطعة أصبحت "يمين" اللاعب بمسافة كبيرة جداً ---
//            // (اللاعب كان يتحرك يساراً)
//            else if (distance > halfTotalWidth)
//            {
//                // انقل القطعة فوراً إلى أقصى اليسار
//                piece.position = new Vector3(piece.position.x - totalWidth, piece.position.y, piece.position.z);
//                // Debug.Log($"Moved {piece.name} LEFT");
//            }
//        }
//    }
//}
#endregion

using System.Collections.Generic;
using UnityEngine;


public class InfiniteMapManager : MonoBehaviour
{
    public static float CurrentMoveVelocity { get; private set; }

    [Header("Map Settings")]
    public List<Transform> mapPieces;
    public float mapWidth = 46.08f;

    [Header("Speed Settings")]
    public float moveSpeed = 5f;
    public float runSpeed = 10f;

    [Header("Player Reference")]
    public Transform playerTransform;

    private float currentSpeed;
    private float moveInput;
    private bool isRunning;
    private Player playerScript;

    private void Start()
    {
        InitializePlayer();
    }

    private void Update()
    {
        ReadInput();
    }

    private void FixedUpdate()
    {
        if (IsGameStopped())
        {
            StopMovement();
            return;
        }

        UpdateSpeed();
        MoveMap();
        CheckLoop();
    }

    #region Initialization

    private void InitializePlayer()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
                playerScript = player.GetComponent<Player>();
            }
            else
            {
                Debug.LogError("Player not found! Please assign Player Transform in Inspector.");
            }
        }
        else
        {
            playerScript = playerTransform.GetComponent<Player>();
        }
    }

    #endregion

    #region Input & Speed

    private void ReadInput()
    {
        moveInput = Input.GetAxis("Horizontal");
        isRunning = Input.GetKey(KeyCode.LeftShift);
    }

    private void UpdateSpeed()
    {
        currentSpeed = isRunning ? runSpeed : moveSpeed;
    }

    private bool IsGameStopped()
    {
        return GameManager.Instance != null && GameManager.Instance.isGameOver;
    }

    #endregion

    #region Movement

    private void StopMovement()
    {
        CurrentMoveVelocity = 0;
    }

    private void MoveMap()
    {
        float moveVelocity = -moveInput * currentSpeed;
        CurrentMoveVelocity = moveVelocity;

        if (moveVelocity == 0) return;

        foreach (Transform piece in mapPieces)
        {
            if (piece != null)
            {
                piece.Translate(moveVelocity * Time.fixedDeltaTime, 0, 0);
            }
        }
    }

    #endregion

    #region Loop Management

    private void CheckLoop()
    {
        if (playerTransform == null) return;

        float referenceX = playerTransform.position.x;
        float totalWidth = mapWidth * mapPieces.Count;
        float halfTotalWidth = totalWidth / 2f;

        foreach (Transform piece in mapPieces)
        {
            if (piece == null) continue;

            float distance = piece.position.x - referenceX;

            if (distance < -halfTotalWidth)
            {
                piece.position = new Vector3(
                    piece.position.x + totalWidth,
                    piece.position.y,
                    piece.position.z
                );
            }
            else if (distance > halfTotalWidth)
            {
                piece.position = new Vector3(
                    piece.position.x - totalWidth,
                    piece.position.y,
                    piece.position.z
                );
            }
        }
    }

    #endregion

    #region Gizmos (للمساعدة في التصميم)

    private void OnDrawGizmosSelected()
    {
        if (playerTransform == null || mapPieces.Count == 0) return;

        Gizmos.color = Color.red;
        float totalWidth = mapWidth * mapPieces.Count;
        float halfTotalWidth = totalWidth / 2f;

        Vector3 leftBound = playerTransform.position + Vector3.left * halfTotalWidth;
        Vector3 rightBound = playerTransform.position + Vector3.right * halfTotalWidth;

        Gizmos.DrawLine(leftBound + Vector3.up * 5, leftBound + Vector3.down * 5);
        Gizmos.DrawLine(rightBound + Vector3.up * 5, rightBound + Vector3.down * 5);
    }

    #endregion
}