def convertMicrobitValue(array):
	
	hexString = ''
	values = array[0]
	
	for value in values:
				
		hexString = str(hex(ord(value)))[2:] + hexString

	return int(hexString,16)


def marshalStringForBleUartSending(strData):
	
	strPreparedData = strData + '\r\n'
	strPreparedData = strPreparedData.encode('utf-8')
	
	return strPreparedData