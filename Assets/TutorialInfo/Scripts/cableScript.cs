using System;
using UnityEngine;

public class cableScript : MonoBehaviour
{
    [Header("Referências")]
    public Transform startPoint;
    public Transform endPoint;
    public LineRenderer lineRenderer;

    [Header("Propriedades do Cabo")]
    public string CaboName;
    public int quantidadeDePontos = 50;
    public float temperaturaInicial;
    public float temperaturaFinal;
    public float resistencia;
    public float sagAmount;  // Flecha (distância que o cabo afunda)
    public float weightPerUnit = 1.8f;

    void Start()
    {
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer não está atribuído!");
        }
    }

    void Update()
    {
        RenderCatenary();
    }

    void RenderCatenary()
{
    if (lineRenderer == null || startPoint == null || endPoint == null) return;

    // Posições iniciais e finais
    Vector3 startPos = startPoint.position;
    Vector3 endPos = endPoint.position;
    float horizontalDistance = new Vector2(endPos.x - startPos.x, endPos.z - startPos.z).magnitude;
    float averageHeight = (startPos.y + endPos.y) / 2f; // Média das alturas dos postes

    // Define a flecha como uma diferença negativa em relação à altura média dos postes
    float lowestPointY = averageHeight - sagAmount;

    // Ajuste do parâmetro 'a' para controlar a curvatura da catenária
    float a = Mathf.Max((weightPerUnit * horizontalDistance) / (8 * sagAmount), 1f);

    lineRenderer.positionCount = quantidadeDePontos;

    for (int i = 0; i < quantidadeDePontos; i++)
    {
        float t = i / (float)(quantidadeDePontos - 1);
        float x = Mathf.Lerp(0, horizontalDistance, t);

        // Cálculo da posição Y ajustado corretamente
        float yOffset = a * (float)Math.Cosh((x - horizontalDistance / 2) / a) - a;
        float y = lowestPointY + yOffset;

        // Interpolação da posição inicial e final
        Vector3 point = Vector3.Lerp(startPos, endPos, t);
        point.y = y; // Ajusta apenas o valor de Y com base na catenária

        lineRenderer.SetPosition(i, point);
    }
}

}
