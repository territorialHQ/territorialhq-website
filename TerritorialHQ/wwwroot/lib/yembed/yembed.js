(function () {

    var embedLinks = document.getElementsByClassName('yembed');

	Array.prototype.filter.call(embedLinks, function (link) {

		link.classList.add("yembed-container");
		link.style.height = (link.offsetWidth * 0.5625) + "px";
		
		var agreementLink = document.createElement("a");
		agreementLink.setAttribute("href", "javascript:void(0);")
		agreementLink.setAttribute("data-link", link.innerHTML)
		agreementLink.innerHTML = '<div class="yembed-play-button"></div>';
		agreementLink.classList.add("yembed-play-button");

		var agreementText = document.createElement("p");
		agreementText.classList.add("yembed-agreement-text");
		agreementText.innerHTML = "By activating the YouTube embed you acknowledge TTHQ's <a href='/Privacy'>privacy policy</a>";

		agreementLink.addEventListener("click", showEmbeddedContent)

		link.innerHTML = "";
		link.appendChild(agreementLink);
		link.appendChild(agreementText);

		function showEmbeddedContent(evt) {

			var container = evt.currentTarget.parentElement;

			var ytLink = evt.currentTarget.getAttribute("data-link");
			ytLink = "https://www.youtube-nocookie.com/embed?listType=playlist&list=" + ytLink;
		
			var videoBox = document.createElement("div");
			videoBox.classList.add("video-container");

			var iframe = document.createElement("iframe");
			iframe.setAttribute("src", ytLink);
			iframe.setAttribute("frameborder", "0");
			iframe.setAttribute("allow", "accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture");
			iframe.setAttribute("allowFullscreen", "");

			videoBox.appendChild(iframe);
			container.innerHTML = "";
			var classList = container.classList;
			while (classList.length > 0) {
				classList.remove(classList.item(0));
			}
			container.classList.add("mt-20");
			container.appendChild(videoBox);
		}
	});

})();