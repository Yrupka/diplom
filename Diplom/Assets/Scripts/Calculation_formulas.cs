using System.Collections.Generic;

public class Calculation_formulas
{
    // функция вычисляющая интерполирующую состовляющую графика, label_x,y - значения исходной функции
    public static float Interpolate(float x, List<int> label_x, List<float> label_y)
    {
        float answ = 0f;
        for (int j = 0; j < label_x.Count; j++)
        {
            float l_j = 1f;
            for (int i = 0; i < label_x.Count; i++)
            {
                if (i == j)
                    l_j *= 1f;
                else
                    l_j *= (x - label_x[i]) / (label_x[j] - label_x[i]);
            }
            answ += l_j * label_y[j];
        }
        return answ;
    }
}
