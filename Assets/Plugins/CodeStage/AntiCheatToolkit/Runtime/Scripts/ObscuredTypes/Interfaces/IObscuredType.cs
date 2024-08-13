#region copyright
// ------------------------------------------------------
// Copyright (C) Dmitriy Yukhanov [https://codestage.net]
// ------------------------------------------------------
#endregion

using CodeStage.AntiCheat.ObscuredTypes.Converters;
using Newtonsoft.Json;

namespace CodeStage.AntiCheat.ObscuredTypes
{
	/// <summary>
	/// Base interface for all obscured types.
	/// </summary>
	///
	[JsonConverter(typeof(ObscuredTypesNewtonsoftConverter))]
	public interface IObscuredType
	{
		/// <summary>
		/// Allows to change current crypto key to the new random value and re-encrypt variable using it.
		/// Use it for extra protection against 'unknown value' search.
		/// Just call it sometimes when your variable doesn't change to fool the cheater.
		/// </summary>
		void RandomizeCryptoKey();
	}
}