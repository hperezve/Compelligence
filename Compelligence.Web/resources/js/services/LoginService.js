/**
 * Login Service
 */
app.service("LoginService", function(PuridiomData, $log) {
	
	this.setPuridiomHeader = function (request) {
		
		PuridiomData.request.requestHeader.module = 'authentication';
		PuridiomData.request.requestHeader.service = 'login';
		PuridiomData.request.requestHeader.pageId = '';
		PuridiomData.request.requestHeader.moduleRequest = '';
		PuridiomData.request.requestHeader.option = '';
		PuridiomData.request.requestHeader.processKey = '';
		PuridiomData.request.requestHeader.browseKey = '';

		$log.debug("Login Service - set request header");
		$log.debug(PuridiomData.request.requestHeader);
	};
	
	this.validEmail = function (request) {
		var mailId = request.requestHeader.identity.mailId;
		if (mailId != undefined) {
			return true;
		} else {
			return false;
		}
	};
	
});