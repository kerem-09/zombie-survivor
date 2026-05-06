using UnityEngine;

public class ZombieAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip zombiHirilti;
    public AudioClip zombiOlum;

    // STATÝK DEÐÝÞKENLER: Tüm zombiler bu verileri ortak takip eder
    private static int toplamZombiSayisi = 0;
    private static int aktifSesSayisi = 0;

    [Header("Disiplin Ayarlarý")]
    public int disiplinEsigi = 11; // Kaç zombiden sonra kýsýtlama baþlasýn?
    public int maxSesLimiti = 3;   // Disiplin modunda ayný anda kaç ses çýksýn?

    void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        toplamZombiSayisi++; // Yeni bir zombi doðdu, sayýyý artýr

        float baslangicGecikmesi = Random.Range(2f, 5f);
        float tekrarSuresi = Random.Range(7f, 12f);

        // Zombi silindiðinde hata vermemesi için güvenli çaðýrma yapýyoruz
        InvokeRepeating("PlayIdleGroan", baslangicGecikmesi, tekrarSuresi);
    }

    void OnDestroy()
    {
        // Önemli: Zombi ölünce toplam sayýdan düþüyoruz ki disiplin sistemi doðru çalýþsýn
        toplamZombiSayisi--;
    }

    void PlayIdleGroan()
    {
        // Eðer AudioSource kapalýysa veya obje pasifse hiç deneme bile
        if (audioSource == null || !audioSource.isActiveAndEnabled) return;

        // --- DÝSÝPLÝN KONTROLÜ ---
        if (toplamZombiSayisi >= disiplinEsigi)
        {
            if (aktifSesSayisi >= maxSesLimiti) return; // Limit doluysa sus
        }

        // Zaten bir ses çalýyorsa yeni ses baþlatma
        if (!audioSource.isPlaying)
        {
            StartCoroutine(SesCalmaSureci());
        }
    }

    System.Collections.IEnumerator SesCalmaSureci()
    {
        // Çalmadan hemen önce son bir güvenlik kontrolü (Hata almaný engeller)
        if (audioSource != null && audioSource.isActiveAndEnabled && zombiHirilti != null)
        {
            aktifSesSayisi++; // Rezerve et

            audioSource.clip = zombiHirilti;
            audioSource.pitch = Random.Range(0.85f, 1.15f);
            audioSource.Play();

            // Ses dosyasý bitene kadar bekle
            yield return new WaitForSeconds(audioSource.clip.length);

            aktifSesSayisi--; // Yer aç
        }
    }

    public void PlayDeathSoundAndDestroy()
    {
        if (zombiOlum != null)
        {
            // AudioSource.PlayClipAtPoint sihirli bir koddur:
            // Zombi yok olsa bile ses o noktada çalmaya devam eder.
            AudioSource.PlayClipAtPoint(zombiOlum, transform.position);
        }
    }
}