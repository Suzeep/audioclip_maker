using UnityEngine;
using System.IO;
using System.Collections;

public class SceneMain : MonoBehaviour
{
	// OnGUI
	void OnGUI()
	{
		float w = Screen.width;
		float h = Screen.height;
		
		if( GUI.Button(new Rect(w*0.40f,h*0.30f,w*0.20f,h*0.15f), "Read") ){
			readFile();
		}
		if( GUI.Button(new Rect(w*0.40f,h*0.50f,w*0.20f,h*0.15f), "Play") ){
			Debug.Log("Play.");
			m_AudioPlayer.GetComponent<AudioSource>().Play();
		}
	}
	
	// read wav file
	private void	readFile()
	{
		string path = string.Format("{0}/Sounds/{1}.wav", Application.dataPath, m_WavFileName);
		byte[] buf = File.ReadAllBytes( path );
		
		// analyze wav file
		m_WavInfo.Analyze( buf );
		
		// create audio clip
		AudioSource source = m_AudioPlayer.GetComponent<AudioSource>();
		source.clip = m_ClipMaker.Create(
			"making_clip",
			buf,
			m_WavInfo.TrueWavBufIdx,
			m_WavInfo.BitPerSample,
			m_WavInfo.TrueSamples,
			m_WavInfo.Channels,
			m_WavInfo.Frequency,
			false,
			false
		);
		
		Debug.Log( string.Format("read {0}.wav", m_WavFileName) );
	}
	
	// member
	public AudioClipMaker	m_ClipMaker;
	public GameObject		m_AudioPlayer;
	private WavFileInfo		m_WavInfo = new WavFileInfo();
	public string			m_WavFileName;
}
