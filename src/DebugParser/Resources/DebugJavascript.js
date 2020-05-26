window.onload = function(){
	console.log("hello");
	var bytes = document.querySelectorAll("span[index]");
	var highlighted = [];
	var toggler = document.getElementsByClassName("caret");
	for (var i = 0; i < toggler.length; i++)
	{
		toggler[i].addEventListener("click", function() {
			this.parentElement.parentElement.querySelector(".nested").classList.toggle("active");
			this.classList.toggle("caret-down");
		});
	}

	var labels = document.getElementsByClassName("tree-label");
	for (var i = 0; i < labels.length; i++)
	{
		labels[i].addEventListener("click", function() {
			var dataStart = parseInt(this.getAttribute("data-start"));
			var dataEnd = parseInt(this.getAttribute("data-end"));
			for(var j = 0; j < highlighted.length; j++){
				highlighted[j].className = "";
			}
			highlighted = [];
			bytes[dataStart].scrollIntoView();
			for(var j = dataStart; j < dataEnd && j < bytes.length; j++){
				bytes[j].className = "highlighted";
				highlighted.push(bytes[j]);
			}
		});
	}
}