using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Gun : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject muzzleFlashPrefab; // Prefab del muzzle flash (chispazo al disparar)
    public float bulletForce = 20f;
    public float normalFireRate = 3f;  // Fire rate normal
    public float boostedFireRate = 30f;  // Fire rate aumentado
    private float fireRate;  // Fire rate actual, puede ser normal o aumentado
    private float nextTimeToFire = 0f;

    // Referencia al AudioSource
    public AudioSource gunshotSound;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private bool isBoosted = false;  // Indica si el fire rate está aumentado
    private float boostDuration = 6.5f;  // Duración del aumento de fire rate en segundos
    private float boostEndTime = 0f;  // Tiempo en el que termina el aumento de fire rate

    private bool canUseR = true;  // Indica si se puede volver a usar la habilidad de "R"
    public RawImage uiIcon;  // Referencia a la RawImage donde se mostrará el icono
    public TextMeshProUGUI boostDurationText; // Referencia al UI Text para mostrar la duración de boost

    // Variables para la animación del icono
    private float minScale = 0.8f;  // Escala mínima del icono
    private float maxScale = 1.1f;  // Escala máxima del icono
    private float scaleSpeed = 1.5f;  // Velocidad de cambio de escala
    private float currentScale = 1f;  // Escala actual del icono

    // Instancia estática para acceder al normalFireRate desde otros scripts
    private static Gun instance;

    public GameObject rAbilityAnimationPrefab; // Prefab de la animación de la habilidad "R"
    private GameObject currentRAbilityAnimation; // Referencia al objeto actual de la animación de "R"
    private RecoilSystem recoilSystem;
    public bool IsAiming;

    void Awake()
    {
        instance = this;
    }

    public static float GetNormalFireRate()
    {
        return instance.normalFireRate;
    }

    public void IncreaseNormalFireRate(float bonus)
    {
        normalFireRate += bonus;  // Aumenta el normalFireRate
        if (!isBoosted)  // Solo actualiza fireRate si no está aumentado
        {
            fireRate = normalFireRate;
        }
    }

    void Start()
    {
        recoilSystem = GetComponent<RecoilSystem>();
        fireRate = normalFireRate;  // Al inicio, el fire rate es normal
        nextTimeToFire = Time.time + 1f / fireRate;  // Calcula el próximo tiempo de disparo inicial

        // Guarda la posición y rotación inicial de la pistola
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        // Asegura que la pistola mantenga su posición y rotación inicial
        transform.localPosition = initialPosition;
        transform.localRotation = initialRotation;

        // Controla la habilidad de "R"
        HandleRAbility();

        // Actualiza el icono en la UI
        UpdateUIIcon();

        // Dispara si está presionado Fire1 y ha pasado el tiempo necesario
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            Shoot();
            recoilSystem.ApplyRecoil();

            // Reproduce el sonido de disparo
            if (gunshotSound != null)
            {
                gunshotSound.Play();
            }

            // Mostrar el muzzle flash
            ShowMuzzleFlash();

            // Calcula el próximo tiempo de disparo basado en el fireRate actual
            nextTimeToFire = Time.time + 1f / fireRate;
        }

        // Actualiza la posición de la animación de "R" si está activa
        UpdateRAbilityAnimationPosition();
    }

    void HandleRAbility()
    {
        // Verifica si se presionó la tecla "R" y se puede usar la habilidad de "R"
        if (Input.GetKeyDown(KeyCode.R) && canUseR)
        {
            IncreaseFireRate();
        }

        // Controla la duración del aumento de fire rate
        if (isBoosted && Time.time >= boostEndTime)
        {
            ResetFireRate();
            StartRCooldown();
        }
    }

    void IncreaseFireRate()
    {
        if (canUseR)
        {
            // Instancia la animación de habilidad "R" en la posición del jugador
            if (rAbilityAnimationPrefab != null)
            {
                currentRAbilityAnimation = Instantiate(rAbilityAnimationPrefab, transform.position, transform.rotation);
            }

            // Duplica el normalFireRate actual
            boostedFireRate = 2 * normalFireRate;

            fireRate = boostedFireRate;  // Asigna el fire rate duplicado
            isBoosted = true;  // Activa la bandera de aumento de fire rate
            boostEndTime = Time.time + boostDuration;  // Calcula el tiempo en el que termina el aumento

            // Desactiva la habilidad de "R" durante el enfriamiento
            canUseR = false;
        }
    }

    public void ResetRAbility()
    {
        canUseR = true;  // Permite usar la habilidad de "R" nuevamente
    }

    void ResetFireRate()
    {
        fireRate = normalFireRate;  // Restaura el fire rate normal
        isBoosted = false;  // Desactiva la bandera de aumento de fire rate
    }

    void StartRCooldown()
    {
        // Inicia el enfriamiento de la habilidad de "R"
        canUseR = false;
    }

    void UpdateUIIcon()
    {
        // Actualiza el icono en la UI para reflejar el estado de la habilidad de "R"
        if (canUseR)
        {
            // Calcula la escala del icono usando una función sinusoidal para el efecto de pulsación
            currentScale = Mathf.Lerp(minScale, maxScale, Mathf.Sin(Time.time * scaleSpeed));
            uiIcon.transform.localScale = new Vector3(currentScale, currentScale, 1f);

            uiIcon.enabled = true;  // Muestra el icono cuando la habilidad está lista
        }
        else
        {
            uiIcon.enabled = false;  // Oculta el icono cuando la habilidad no está lista
        }

        // Actualiza el texto de duración de boost
        if (boostDurationText != null)
        {
            boostDurationText.text = boostDuration.ToString() + "s"; // Agrega "s" al final del texto de duración de boost
        }
    }

    void ShowMuzzleFlash()
    {
        // Instancia el muzzle flash en la posición del fire point
        GameObject muzzleFlash = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation);
        // Asigna el cañón del arma como padre para que siga su posición y rotación
        muzzleFlash.transform.parent = firePoint;
        // Desasigna el padre después de un pequeño retraso para evitar que siga girando con el arma
        Invoke(nameof(UnparentMuzzleFlash), 0.05f);
    }

    void UnparentMuzzleFlash()
    {
        // Desasigna el padre del muzzle flash para que no siga girando con el arma
        foreach (Transform child in firePoint.transform)
        {
            if (child.CompareTag("MuzzleFlash"))
            {
                child.parent = null;
                break;
            }
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("El prefab de la bala no tiene un Rigidbody");
        }
    }

    void UpdateRAbilityAnimationPosition()
    {
        // Actualiza la posición y rotación de la animación de "R" si está activa
        if (currentRAbilityAnimation != null)
        {
            currentRAbilityAnimation.transform.position = transform.position;
            currentRAbilityAnimation.transform.rotation = transform.rotation;

        }
    }
    public void IncreaseBoostDuration(float bonusDuration)
    {
        boostDuration += bonusDuration;  // Aumenta la duración de boost
        UpdateUIBoostDuration();  // Actualiza la UI para reflejar el cambio
    }

    void UpdateUIBoostDuration()
    {
        if (boostDurationText != null)
        {
            boostDurationText.text = boostDuration.ToString("F1") + "s"; // Actualiza el texto con un decimal
        }
    }

}
