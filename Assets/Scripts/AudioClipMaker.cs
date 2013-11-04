using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioClipMaker : MonoBehaviour
{
	// constant
	public readonly	float	RANGE_VALUE_BIT_8		= 1.0f / Mathf.Pow(2,7);	// 1 / 128
	public readonly	float	RANGE_VALUE_BIT_16		= 1.0f / Mathf.Pow(2,15);	// 1 / 32768
	public const int		BASE_CONVERT_SAMPLES	= 1024 * 20;
	public const int		BIT_8					= 8;
	public const int		BIT_16					= 16;
	
	//---------------------------------------------------------------------------
	// create AudioClip by binary raw data
	//---------------------------------------------------------------------------
	public AudioClip	Create(
		string	name,
		byte[]	raw_data,
		int		wav_buf_idx,
		int		bit_per_sample,
		int		samples,
		int		channels,
		int		frequency,
		bool	is3D,
		bool	isStream
	)
	{
		// convert to ranged_raw_data from byte_raw_data
		float[] ranged_raw_data = CreateRangedRawData( raw_data, wav_buf_idx, samples, channels, bit_per_sample );
		
		// create clip and set
		return Create( name, ranged_raw_data, samples, channels, frequency, is3D, isStream );
	}
	
	//---------------------------------------------------------------------------
	// create AudioClip by ranged raw data
	//---------------------------------------------------------------------------
	public AudioClip	Create(
		string	name,
		float[]	ranged_data,
		int 	samples,
		int 	channels,
		int		frequency,
		bool	is3D,
		bool	isStream
	)
	{
		AudioClip clip = AudioClip.Create( name, samples, channels, frequency, is3D, isStream );
		// set data to clip
		clip.SetData( ranged_data, 0 );
		
		return clip;
	}
	
	//---------------------------------------------------------------------------
	// create rawdata( ranged 0.0 - 1.0 ) from binary wav data
	//---------------------------------------------------------------------------
	public float[]		CreateRangedRawData( byte[] byte_data, int wav_buf_idx, int samples, int channels, int bit_per_sample )
	{
		float[] ranged_rawdata = new float[ samples * channels ];
		
		int	step_byte	= bit_per_sample / BIT_8;
		int	now_idx		= wav_buf_idx;
		
		for( int i=0; i < (samples * channels); ++i )
		{
			ranged_rawdata[ i ] = convertByteToFloatData( byte_data, now_idx, bit_per_sample );

			now_idx += step_byte;
		}
		
		return ranged_rawdata;
	}
	
	//---------------------------------------------------------------------------
	// convert byte data to float data
	//---------------------------------------------------------------------------
	private float	convertByteToFloatData( byte[] byte_data, int idx, int bit_per_sample )
	{
		float float_data = 0.0f;
		
		switch( bit_per_sample )
		{
			case BIT_8:
			{
				float_data = ((int)byte_data[idx] - 0x80) * RANGE_VALUE_BIT_8;
			}
				break;
			case BIT_16:
			{
				short sample_data = System.BitConverter.ToInt16( byte_data, idx );
				float_data = sample_data * RANGE_VALUE_BIT_16;
			}
				break;
		}
		
		return float_data;
	}
}
