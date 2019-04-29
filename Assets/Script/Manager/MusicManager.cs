using CleverCode;
using UnityEngine.SceneManagement;
using System;
using UnityEngine;

public class MusicManager : Singleton<MusicManager>
{
    private enum Scenes
    {
        MainMenu = 0,
        ShopMenu = 1,
        GameScene = 2
    }
    private Scenes currentScene;
    private Scenes previousScene;

    [SerializeField] private AudioSource _menuMusic;
    [SerializeField] private AudioSource _GameMusicSlow;
    [SerializeField] private AudioSource _GameMusicMedium;
    [SerializeField] private AudioSource _GameMusicFast;

    public void SetScene(int sceneId)
    {
        if (Enum.IsDefined(typeof(Scenes), sceneId))
        {
            previousScene = currentScene;
            currentScene = (Scenes)sceneId;
            switch (currentScene)
            {
                case Scenes.MainMenu:
                    if (previousScene != Scenes.ShopMenu)
                    {
                        ToMenu();
                    }
                    break;
                case Scenes.ShopMenu:
                    if (previousScene != Scenes.MainMenu)
                    {
                        ToMenu();
                    }
                    break;
                case Scenes.GameScene:
                    ToGame();
                    break;
            }
        }
    }

    private void ToMenu()
    {
        _GameMusicSlow.Stop();
        _GameMusicMedium.Stop();
        _GameMusicFast.Stop();
        _menuMusic.Play();
    }
    private void ToGame()
    {
        _menuMusic.Stop();
        _GameMusicSlow.Play();
        _GameMusicSlow.mute = false;
        _GameMusicMedium.Play();
        _GameMusicFast.Play();
    }

    public void setPlayerSpeed(float playerSpeed)
    {
        if (playerSpeed >= 17)
        {
            _GameMusicFast.volume = 1;
            _GameMusicMedium.volume = 1;
            
            _GameMusicFast.mute = false;
            _GameMusicMedium.mute = false;
            _GameMusicSlow.mute = false;
        }
        else if (playerSpeed >= 13)
        {
            float FastVolume = (playerSpeed - 13) / 4.0f;
            _GameMusicFast.volume = FastVolume;
            _GameMusicMedium.volume = 1;

            _GameMusicFast.mute = false;
            _GameMusicMedium.mute = false;
            _GameMusicSlow.mute = false;
        }
        else if (playerSpeed >= 6)
        {
            _GameMusicMedium.volume = 1;

            _GameMusicFast.mute = true;
            _GameMusicMedium.mute = false;
            _GameMusicSlow.mute = false;
        }
        else if (playerSpeed >= 4)
        {
            float MedVolume = (playerSpeed - 4) / 2.0f;
            _GameMusicMedium.volume = MedVolume;

            _GameMusicFast.mute = true;
            _GameMusicMedium.mute = false;
            _GameMusicSlow.mute = false;
        }
        else
        {
            _GameMusicFast.mute = true;
            _GameMusicMedium.mute = true;
            _GameMusicSlow.mute = false;
        }
    }

    public void Pause()
    {
        _GameMusicFast.Pause();
        _GameMusicMedium.Pause();
        _GameMusicSlow.Pause();
    }

    public void UnPause()
    {
        _GameMusicFast.UnPause();
        _GameMusicMedium.UnPause();
        _GameMusicSlow.UnPause();
    }
}
