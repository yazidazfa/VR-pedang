using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class SwordScaler : MonoBehaviour
{
    public float scaleFactor = 0.1f; // Faktor untuk memperbesar atau memperkecil
    private Vector3 originalScale;    // Menyimpan skala awal pedang
    private bool isGrabbed = false;   // Menyimpan status apakah pedang sedang di-grab

    private XRGrabInteractable grabInteractable; // Referensi ke komponen XRGrabInteractable
    private XRController controller; // Referensi ke controller (Kiri atau Kanan)

    private void Start()
    {
        originalScale = transform.localScale; // Menyimpan skala asli pedang
        grabInteractable = GetComponent<XRGrabInteractable>(); // Mendapatkan referensi ke XRGrabInteractable

        if (grabInteractable != null)
        {
            grabInteractable.onSelectEntered.AddListener(OnGrab); // Mendengarkan saat pedang di-grab
            grabInteractable.onSelectExited.AddListener(OnRelease); // Mendengarkan saat pedang dilepas
        }
        else
        {
            Debug.LogError("XRGrabInteractable tidak ditemukan pada objek pedang.");
        }
    }

    private void Update()
    {
        // Menggunakan tombol A untuk memperbesar dan B untuk memperkecil hanya jika pedang di-grab
        if (isGrabbed)
        {
            // Deteksi input untuk tombol A (misalnya PrimaryButton) dan B (SecondaryButton)
            if (controller.inputDevice.IsPressed(InputHelpers.Button.PrimaryButton, out bool isPressedA) && isPressedA)
            {
                TryScale(Vector3.one * scaleFactor);  // Memperbesar pedang
            }
            if (controller.inputDevice.IsPressed(InputHelpers.Button.SecondaryButton, out bool isPressedB) && isPressedB)
            {
                TryScale(Vector3.one * -scaleFactor);  // Memperkecil pedang
            }
        }
    }

    // Menangani event saat pedang di-grab
    private void OnGrab(XRBaseInteractor interactor)
    {
        isGrabbed = true; // Tandai bahwa pedang di-grab
        controller = interactor.GetComponent<XRController>(); // Menyimpan referensi ke controller yang digunakan
        Debug.Log("Pedang di-grab.");
    }

    // Menangani event saat pedang dilepas
    private void OnRelease(XRBaseInteractor interactor)
    {
        isGrabbed = false; // Tandai bahwa pedang tidak lagi di-grab
        controller = null; // Hapus referensi ke controller
        Debug.Log("Pedang dilepas.");
    }

    // Coba untuk mengubah skala pedang
    private void TryScale(Vector3 scaleChange)
    {
        transform.localScale += scaleChange;

        // Cek jika skala terlalu kecil atau negatif
        if (transform.localScale.x < 0.1f)
        {
            transform.localScale = originalScale; // Reset skala ke nilai awal
        }

        Debug.Log("Pedang di-scaling ke: " + transform.localScale);
    }
}
