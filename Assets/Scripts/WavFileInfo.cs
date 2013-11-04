using UnityEngine;
using System;
using System.IO;
using System.Text;

public class WavFileInfo
{
	//---------------------------------------------------------------------------
	// analyze from buffer
	//---------------------------------------------------------------------------
	public void		Analyze( byte[] buffer )
	{
		FileSize = buffer.Length;
		
		int byte_idx = 0;
		
		// RIFF (4byte)
		RIFF = Encoding.ASCII.GetString( buffer, byte_idx, 4 );
		byte_idx += 4;
		
		// byte size without RIFF (4byte)
		AfterSize = BitConverter.ToInt32( buffer, byte_idx );
		byte_idx += 4;
		
		// WAVE (4byte)
		WAVE = Encoding.ASCII.GetString( buffer, byte_idx, 4 );
		byte_idx += 4;
		
		// f m t ''
		FMT = Encoding.ASCII.GetString( buffer, byte_idx, 4 );
		byte_idx += 4;
		
		// fmt chunk size (4byte)
		FmtChunkSize = BitConverter.ToInt32( buffer, byte_idx );
		byte_idx += 4;
		
		// format id (2byte)
		FormatId = BitConverter.ToInt16( buffer, byte_idx );
		byte_idx += 2;
		
		// channels (2byte)
		Channels = BitConverter.ToInt16( buffer, byte_idx );
		byte_idx += 2;
		
		// frequency : sampling rate (4byte)
		Frequency = BitConverter.ToInt32( buffer, byte_idx );
		byte_idx += 4;
		
		// data speed (4byte)
		DataSpeed = BitConverter.ToInt32( buffer, byte_idx );
		byte_idx += 4;
		
		// block size (2byte)
		BlockSize = BitConverter.ToInt16( buffer, byte_idx );
		byte_idx += 2;
		
		// bit / sample (2byte)
		BitPerSample = BitConverter.ToInt16( buffer, byte_idx );
		byte_idx += 2;
		
		// format info size through
		byte_idx += (FmtChunkSize - 16);
		
		// FLLR chunk check (4byte)
		string check = Encoding.ASCII.GetString( buffer, byte_idx, 4 );
		if( check.Equals("FLLR") )
		{
			IsFLLR = true;
			
			// check FLLR block size
			byte_idx += 4;
			int block_size = BitConverter.ToInt32( buffer, byte_idx );
			byte_idx += (4 + block_size);	// skip
		}
		else{
			IsFLLR = false;
		}
		
		// JUNK chunk check
		check = Encoding.ASCII.GetString( buffer, byte_idx, 4 );
		if( check.Equals("JUNK") )
		{
			IsJUNK = true;
			
			// check junk block size
			byte_idx += 4;
			int block_size = BitConverter.ToInt32( buffer, byte_idx );
			byte_idx += (4 + block_size);
		}
		else{
			IsJUNK = false;
		}
		
		// d a t a (4byte)
		DATA = Encoding.ASCII.GetString( buffer, byte_idx, 4 );
		byte_idx += 4;
		
		// true wave buffer size (4byte)
		TrueWavBufSize	= BitConverter.ToInt32( buffer, byte_idx );
		TrueSamples		= TrueWavBufSize / (BitPerSample / AudioClipMaker.BIT_8) / Channels;
		byte_idx += 4;
		
		TrueWavBufIdx = byte_idx;
		
		// (after wav data)
	}
	
	//---------------------------------------------------------------------------
	// member
	//---------------------------------------------------------------------------
	public int		FileSize {
		get;
		private set;
	}
	public string	RIFF {
		get;
		private set;
	}
	public int		AfterSize {
		get;
		private set;
	}
	public string	WAVE {
		get;
		private set;
	}
	public string	FMT {
		get;
		private set;
	}
	public int		FmtChunkSize {
		get;
		private set;
	}
	public int		FormatId {
		get;
		private set;
	}
	public int		Channels {
		get;
		private set;
	}
	public int		Frequency {
		get;
		private set;
	}
	public int		DataSpeed {
		get;
		private set;
	}
	public int		BlockSize {
		get;
		private set;
	}
	public int		BitPerSample {
		get;
		private set;
	}
	public bool		IsFLLR {
		get;
		private set;
	}
	public bool		IsJUNK {
		get;
		private set;
	}
	public string	DATA {
		get;
		private set;
	}
	public int		TrueWavBufSize {
		get;
		private set;
	}
	public int		TrueWavBufIdx {
		get;
		private set;
	}
	public int		TrueSamples {
		get;
		private set;
	}
}
