using UnityEngine;

public class FireShell : MonoBehaviour
{
    public GameObject bullet;
    public GameObject turret;
    public GameObject enemy;

    void CreateBullet()
    {
        Instantiate(bullet, turret.transform.position, turret.transform.rotation);
    }

    Vector3 CalculateTrajectory()
    {
        if (enemy == null) return this.transform.forward;

        Vector3 p = enemy.transform.position - this.transform.position;
        Vector3 v = enemy.transform.forward * enemy.GetComponent<Drive>().speed;
        float s = bullet.GetComponent<MoveShell>().speed;

        float a = Vector3.Dot(v, v) - (s * s);
        float b = Vector3.Dot(p, v);
        float c = Vector3.Dot(p, p);
        float d = b * b - a * c;

        // Alterado de 0.1f para 0f para evitar rejeitar interceções válidas
        if (d < 0f) return Vector3.zero;

        float sqrt = Mathf.Sqrt(d);
        float t1 = (-b - sqrt) / a;
        float t2 = (-b + sqrt) / a;

        float t = 0;
        if (t1 < 0 && t2 < 0) return Vector3.zero;
        else if (t1 < 0) t = t2;
        else if (t2 < 0) t = t1;
        else t = Mathf.Max(t1, t2); // Sintaxe simplificada

        return (t * v) + p;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (enemy != null)
            {
                Vector3 aimAT = CalculateTrajectory();

                if (aimAT != Vector3.zero)
                {
                    // Roda a torre (de onde a bala sai) em vez do tanque inteiro
                    turret.transform.forward = aimAT;

                    // Colocado dentro do IF: só dispara se houver uma trajetória válida
                    CreateBullet();
                }
            }
        }
    }
}