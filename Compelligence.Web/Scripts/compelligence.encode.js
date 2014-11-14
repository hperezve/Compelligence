function decimalToHex(i) 
{
  var result=i.toString(16);
  if ( result.length== 1)
   return "0"+result;
  return result


}


function Encode(value,offset)
{
        var _raw = value;
        var _key = "{Gateway:Compelligence:eBsSAC.}";
        var _encrypted = "";
		var _encoded = "";
		

            var tStr;
            var keyPtr, tempVal, tempKey;

            keyPtr = offset;

            for (i = 0; i < _raw.length; i++)
            {
                var tempVal = _raw.charCodeAt(i);
                var tempKey = _key.charCodeAt(keyPtr);
                tempVal += tempKey;

                while (tempVal > 255)
                {
                    if (tempVal > 255)
                    {
                        tempVal = tempVal - 255;
                    }
                }

                tStr = String.fromCharCode(tempVal);
                _encrypted = _encrypted + tStr;
                var hexStr = decimalToHex(tempVal);

                _encoded+=hexStr;

                keyPtr++;

                if (keyPtr == _key.length)
                {
                    keyPtr = 0;
                }
            }

            _encoded = offset + _encoded

  return _encoded;
}
