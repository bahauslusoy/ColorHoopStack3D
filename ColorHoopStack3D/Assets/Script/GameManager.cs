using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    enum State
    {
        preGame,

        inGame,





    }
    GameObject SeciliObje;
    GameObject SeciliStand;
    Cember _Cember;
    public bool HareketVar;
    public int HedefStandSayisi;
    int TamamlananStandSayisi;

    public AudioSource[] Sesler;
    public TextMeshProUGUI LevelAd;
    public GameObject BolumPaneli;
    [SerializeField] private GameObject StartPanel;



    private State _currentState = State.preGame;

    private void Start()
    {
        StartPanel.SetActive(true);
        Time.timeScale = 0;

        LevelAd.text = "LEVEL : " + SceneManager.GetActiveScene().buildIndex;
    }
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {

            switch (_currentState)
            {
                case State.preGame:
                    if (Input.GetMouseButtonDown(0))
                    {
                        Time.timeScale = 1;


                        StartPanel.SetActive(false);

                        _currentState = State.inGame;

                    }

                    break;

                case State.inGame:

                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100))
                    {
                        if (hit.collider != null && hit.collider.CompareTag("Stand"))
                        {

                            if (SeciliObje != null && SeciliStand != hit.collider.gameObject)
                            {// bir çemberi gönderme

                                Stand _Stand = hit.collider.GetComponent<Stand>();


                                if (_Stand._Cemberler.Count != 4 && _Stand._Cemberler.Count != 0)
                                {

                                    if (_Cember.Renk == _Stand._Cemberler[_Stand._Cemberler.Count - 1].GetComponent<Cember>().Renk)
                                    {
                                        SeciliStand.GetComponent<Stand>().SoketDegistirmeIslemleri(SeciliObje);
                                        _Cember.HareketEt("PozisyonDegistir", hit.collider.gameObject, _Stand.MusaitSoketiVer(), _Stand.HareketPozisyonu);
                                        _Stand.BosOlanSoket++;
                                        _Stand._Cemberler.Add(SeciliObje);
                                        _Stand.CemberleriKontrolEt();
                                        SeciliObje = null;
                                        SeciliStand = null;
                                    }
                                    else
                                    {
                                        _Cember.HareketEt("SoketeGeriGit");
                                        SeciliObje = null;
                                        SeciliStand = null;
                                    }
                                }
                                else if (_Stand._Cemberler.Count == 0)
                                {
                                    SeciliStand.GetComponent<Stand>().SoketDegistirmeIslemleri(SeciliObje);
                                    _Cember.HareketEt("PozisyonDegistir", hit.collider.gameObject, _Stand.MusaitSoketiVer(), _Stand.HareketPozisyonu);
                                    _Stand.BosOlanSoket++;
                                    _Stand._Cemberler.Add(SeciliObje);
                                    _Stand.CemberleriKontrolEt();
                                    SeciliObje = null;
                                    SeciliStand = null;
                                }
                                else
                                {
                                    _Cember.HareketEt("SoketeGeriGit");
                                    SeciliObje = null;
                                    SeciliStand = null;
                                }

                            }
                            else if (SeciliStand == hit.collider.gameObject)
                            {
                                _Cember.HareketEt("SoketeGeriGit");
                                SeciliObje = null;
                                SeciliStand = null;
                            }
                            else
                            {
                                Stand _Stand = hit.collider.GetComponent<Stand>();
                                SeciliObje = _Stand.EnUsttekiCemberiVer();
                                _Cember = SeciliObje.GetComponent<Cember>();
                                HareketVar = true;

                                if (_Cember.HareketEdebilirmi)
                                {
                                    _Cember.HareketEt("Secim", null, null, _Cember._AitOlduguStand.GetComponent<Stand>().HareketPozisyonu);

                                    SeciliStand = _Cember._AitOlduguStand;
                                }
                            }

                        }
                    }
                    break;

            }
        }
    }
    public void StandTamamlandi()
    {
        TamamlananStandSayisi++;
        if (TamamlananStandSayisi == HedefStandSayisi)
            Kazandin();
    }
    public void SesOynat(int Index)
    {
        Sesler[Index].Play();
    }

    void Kazandin()
    {
        BolumPaneli.SetActive(true);
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        Time.timeScale = 0;
    }

    public void Butonlarinislemleri(string Deger)
    {
        switch (Deger)
        {
            case "Tekrar":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                Time.timeScale = 1;
                break;
            case "SonrakiLevel":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                Time.timeScale = 1;
                break;
            case "Ayarlar":
                // ayarlar paneli yapılabilir. sana bırakıyorum
                break;

            case "cikis":
                Application.Quit(); // emin msin paneli ypaıalbilir. Run controlde yaptık.
                break;
        }
    }
}
