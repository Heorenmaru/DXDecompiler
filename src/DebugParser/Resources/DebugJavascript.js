function addDetailLabel(ul, name, value){
	var li = document.createElement("ul");
	ul.appendChild(li);
	li.innerText = `${name}: ${value}`;
	return li;
}
function setDetailPanel(element, detailView){
	const numberTypes = new Set(["UInt32", "Int32", "UInt16", "Int16", "Byte"]);
	var isNumeric = numberTypes.has(element.getAttribute("type"));
	var intValue = isNumeric ? parseInt(element.getAttribute("value")) : 0;
	detailView.innerHTML = "";
	var ul = document.createElement("ul");
	detailView.appendChild(ul);
	addDetailLabel(ul, "Name", element.getAttribute("name"));
	addDetailLabel(ul, "Type", element.getAttribute("type"));
	addDetailLabel(ul, "Value", element.getAttribute("value"));
	if(isNumeric){
		addDetailLabel(ul, "Hex", `0x${intValue.toString(16)}`);
		var ascii = "";
		for(var i = 0; i < 4; i++){
			var byte = (intValue >> (i * 8)) & 0xFF;
			ascii += byte >= 32 && byte <= 126 ? 
				String.fromCharCode(byte) : ".";
		}
		addDetailLabel(ul, "Ascii", ascii);
	}
	addDetailLabel(ul, "Size", element.getAttribute("size"));
	addDetailLabel(ul, "AbsStart", element.getAttribute("data-start"));
	addDetailLabel(ul, "AbsStart", element.getAttribute("data-end"));
	addDetailLabel(ul, "RelStart", element.getAttribute("rel-start"));
	addDetailLabel(ul, "RelEnd", element.getAttribute("rel-end"));
	if(isNumeric){
		var offset = addDetailLabel(ul, "Offset", `${intValue}(0x${intValue.toString(16)})`);
		offset.onclick = function(){
			var hexByte = document.getElementById("b" + intValue);
			hexByte.click();
		}
	}

}
window.onload = function(){
	console.log("hello");
	var detailView = document.getElementById("detailview");
	var bytes = document.querySelectorAll("span[index]");
	var highlighted = [];
	var toggler = document.getElementsByClassName("caret");
	var highlightedMember = null;
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
			if(highlightedMember != null){
				highlightedMember.classList.remove('highlighted');
			}
			this.classList.add("highlighted");
			highlightedMember = this;
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
			setDetailPanel(this, detailView);
		});
	}
	for (var i = 0; i < bytes.length; i++)
	{
		bytes[i].addEventListener("click", function() {
			var memberId = this.getAttribute("member");
			var member = document.getElementById(memberId);
			member.scrollIntoView();
			member.click();
			var parent = member;
			while(parent != null && parent != window){
				if(parent.classList.contains("nested") && !parent.classList.contains("active")){
					parent.classList.toggle("active");
					parent.previousElementSibling.firstElementChild.classList.toggle("caret-down");
				}
				parent = parent.parentElement;
			}
		});
	}
}