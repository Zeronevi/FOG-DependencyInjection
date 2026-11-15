using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class TileMapEditor : MonoBehaviour
{
    //A assinatura da nossa operação leva em consideração a célula clicada no tilemap
    public UnityAction<Vector3Int> selectedAction; // Funções atribuidas devem ser void Funcao(Vector3Int atributo);

    private Tilemap tilemap;

    //Ação de click do novo Input System.
    [SerializeField] private InputAction clickAction = new InputAction("Click", InputActionType.Button, "<Pointer>/press");

    //Lista reaproveitada para os resultados do raycast de UI (evita alocação a cada clique)
    private readonly List<RaycastResult> uiRaycastResults = new List<RaycastResult>();


    void Start()
    {
        tilemap = GetComponentInChildren<Tilemap>();
    }

    void OnEnable()
    {
        clickAction.performed += OnClick;
        clickAction.Enable();
    }

    void OnDisable()
    {
        clickAction.performed -= OnClick;
        clickAction.Disable();
    }

    void OnClick(InputAction.CallbackContext context)
    {
        handleClick();
    }

    //Posição atual do ponteiro na tela, pelo novo Input System
    Vector2 GetPointerPosition()
    {
        return Pointer.current != null ? Pointer.current.position.ReadValue() : Vector2.zero;
    }

    //Chamada pra lidar com o click do usuário no tilemap
    void handleClick()
    {
        Vector2 screenPosition = GetPointerPosition();

        //Se o click caiu em cima de algum menu/UI, ignora
        if (IsPointerOverUI(screenPosition))
            return;

        //Calcula o lugar do mundo onde o click aconteceu.
        //Usamos um raio da câmera contra o plano do tilemap, assim funciona
        //tanto com câmera ortográfica quanto perspectiva.
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Plane tilemapPlane = new Plane(-transform.forward, transform.position);

        if (!tilemapPlane.Raycast(ray, out float distance))
            return;

        Vector3 worldPosition = ray.GetPoint(distance);

        //Converte a posição do mundo para a célula (tile) correspondente
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);

        if (selectedAction != null)
            selectedAction.Invoke(cellPosition);
    }

    //Faz um raycast na UI (via EventSystem) na posição do ponteiro
    bool IsPointerOverUI(Vector2 screenPosition)
    {
        if (EventSystem.current == null)
            return false;

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition
        };

        uiRaycastResults.Clear();
        EventSystem.current.RaycastAll(pointerData, uiRaycastResults);
        return uiRaycastResults.Count > 0;
    }
}
