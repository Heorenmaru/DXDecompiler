function addDetailLabel(ul, name, value){
	var num = parseInt(value);
	var li = document.createElement("ul");
	ul.appendChild(li);
	if(isNaN(num)){
		li.innerText = `${name}: ${value}`;
	} else {
		li.innerText = `${name}: ${value} (0x${num.toString(16)})`;
	}
	return li;
}
function getAllSiblings(elem, filter) {
    var sibs = [];
    elem = elem.parentNode.firstChild;
    do {
        if (elem.nodeType === 3) continue; // text node
        if (!filter || filter(elem)) sibs.push(elem);
    } while (elem = elem.nextSibling)
    return sibs;
}
function getChildrenTreeLabels(element){
	var treeItem = element.parentElement;
	var siblings = getAllSiblings(treeItem, (ele) => ele.tagName == "UL");
	if(siblings.length === 0) {
		return [];
	}
	var listItems = siblings[0].children;
	var result = [];
	for(var i = 0; i < listItems.length; i++){
		var treeItem = listItems[i].children[0];
		var treeLabel = treeItem.getElementsByClassName("tree-label")[0];
		result.push(treeLabel);
	}
	return result;
}
function setDetailPanel(element, detailView){
	const numberTypes = new Set(["UInt32", "Int32", "UInt16", "Int16", "Byte"]);
	var type = element.getAttribute("type");
	var isNumeric = numberTypes.has(type);
	var intValue = isNumeric ? parseInt(element.getAttribute("value")) : 0;
	detailView.innerHTML = "";
	var ul = document.createElement("ul");
	detailView.appendChild(ul);
	addDetailLabel(ul, "Name", element.getAttribute("name"));
	addDetailLabel(ul, "Type", element.getAttribute("type"));
	if (type === "Byte[]") {
		isNumeric = true;
		var index = parseInt(element.getAttribute("data-start"));
		for (var i = 0; i < 4; i++) {
			var b = document.getElementById(`b${index + i}`);
			intValue += parseInt(b.textContent, 16) << i * 8;
		}
		addDetailLabel(ul, "Value", intValue.toString());
	} else {
		addDetailLabel(ul, "Value", element.getAttribute("value"));
	}
	if(isNumeric){
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
	addDetailLabel(ul, "AbsEnd", element.getAttribute("data-end"));
	addDetailLabel(ul, "RelStart", element.getAttribute("rel-start"));
	addDetailLabel(ul, "RelEnd", element.getAttribute("rel-end"));
	if(isNumeric){
		var offset = addDetailLabel(ul, "Offset", `${intValue}(0x${intValue.toString(16)})`);
		offset.onclick = function(){
			var hexByte = document.getElementById("b" + intValue);
			hexByte.click();
		}
	}
	var notesText = element.getAttribute("notes");
	var notes = notesText.split(";");
	for (var note of notes) {
		var pair = note.split(": ");
		if (pair.length === 2) {
			addDetailLabel(ul, pair[0], pair[1]);
		}
	}
}
function isScrolledIntoView(el)
{
    var rect = el.getBoundingClientRect();
    var elemTop = rect.top;
    var elemBottom = rect.bottom;

    // Only completely visible elements return true:
    var isVisible = (elemTop >= 0) && (elemBottom <= window.innerHeight);
    // Partially visible elements return true:
    //isVisible = elemTop < window.innerHeight && elemBottom >= 0;
    return isVisible;
}
function setDetailPanelUnusedMemory(element, detailView) {
	var elements = [];
	var sibling = element;
	var intValue = 0;
	for(var i = 0; i < 4; i++){
		if (sibling !== null) {
			elements.push(sibling);
			intValue += parseInt(sibling.textContent, 16) << i * 8;
		} else {
			break;
		}
		sibling = sibling.nextSibling.nextSibling;
	}
	detailView.innerHTML = "";
	var ul = document.createElement("ul");
	detailView.appendChild(ul);
	addDetailLabel(ul, "Name", "UnreadMemory");
	addDetailLabel(ul, "Type", "UnreadMemory");
	addDetailLabel(ul, "Value", intValue.toString());

	var ascii = "";
	for (var i = 0; i < 4; i++) {
	var byte = (intValue >> (i * 8)) & 0xFF;
	ascii += byte >= 32 && byte <= 126 ?
		String.fromCharCode(byte) : ".";
	}
	addDetailLabel(ul, "Ascii", ascii);

	addDetailLabel(ul, "Size", 4);
	addDetailLabel(ul, "AbsStart", element.getAttribute("index"));
	addDetailLabel(ul, "AbsEnd", element.getAttribute("index") + 4);
	addDetailLabel(ul, "RelStart", element.getAttribute("index"));
	addDetailLabel(ul, "RelEnd", element.getAttribute("index") + 4);

	var offset = addDetailLabel(ul, "Offset", `${intValue}(0x${intValue.toString(16)})`);
	offset.onclick = function() {
	var hexByte = document.getElementById("b" + intValue);
	hexByte.click();
	}
}
class Manager {
	constructor() {
		this.detailView = document.getElementById("detailview");
		this.bytes = document.getElementsByClassName("hex_byte");
		this.ascii = document.getElementsByClassName("hex_ascii");
		this.highlighted = [];
		this.toggler = document.getElementsByClassName("caret");
		this.labels = document.getElementsByClassName("tree-label");
		this.highlightClasses = [];
		for(var i = 0; i < 6; i++){
			this.highlightClasses.push(`highlighted_${i}`);
		}
		for (var i = 0; i < this.toggler.length; i++)
		{
			this.toggler[i].addEventListener("click", this.onTogglerClick.bind(this, this.toggler[i]));
		}
		for (var i = 0; i < this.labels.length; i++)
		{
			this.labels[i].addEventListener("click", this.onLabelClick.bind(this, this.labels[i]));
		}
			for (var i = 0; i < this.bytes.length; i++)
		{
			this.bytes[i].addEventListener("click", this.onByteClick.bind(this, this.bytes[i]));
		}
	}
	onTogglerClick(element, evt){
		element.parentElement.parentElement.querySelector(".nested").classList.toggle("active");
		element.classList.toggle("caret-down");
	}
	onLabelClick(element, evt){
		this.clearHighlighted();
		element.classList.add("highlighted");
		this.highlighted.push([element, "highlighted"]);
		var dataStart = parseInt(element.getAttribute("data-start"));
		var dataEnd = parseInt(element.getAttribute("data-end"));
		if(!isScrolledIntoView(this.bytes[dataStart])){
			this.bytes[dataStart].scrollIntoView();
		}
		var children = getChildrenTreeLabels(element);
		if(children.length != 0) {
			for(var i = 0; i < children.length; i++){
				var treeLabel = children[i];
				var itemStart = parseInt(treeLabel.getAttribute("data-start"));
				var itemEnd = parseInt(treeLabel.getAttribute("data-end"));
				var classIndex = i % this.highlightClasses.length;
				this.highlightByteRange(itemStart, itemEnd, this.highlightClasses[classIndex]);
			}
		} else {
			this.highlightByteRange(dataStart, dataEnd, "highlighted");
		}
		setDetailPanel(element, this.detailView);
	}
	onByteClick(element, evt){
		var memberId = element.getAttribute("member");
		var member = document.getElementById(memberId);
		if (member === null) {
			this.clearHighlighted();
			setDetailPanelUnusedMemory(element, detailView);
			return;
		}
		if(!isScrolledIntoView(member)){
			member.scrollIntoView();
		}
		member.click();
		var parent = member;
		while(parent !== null && parent !== window){
			if(parent.classList.contains("nested") && !parent.classList.contains("active")){
				parent.classList.toggle("active");
				parent.previousElementSibling.firstElementChild.classList.toggle("caret-down");
			}
			parent = parent.parentElement;
		}
	}
	highlightByteRange(start, end, className){
		for(var j = start; j < end && j < this.bytes.length; j++){
			this.bytes[j].classList.add(className);
			this.highlighted.push([this.bytes[j], className]);
			this.ascii[j].classList.add(className);
			this.highlighted.push([this.ascii[j], className]);
		}
	}
	clearHighlighted(){
		for(var i = 0; i < this.highlighted.length; i++){
			let pair = this.highlighted[i];
			pair[0].classList.remove(pair[1]);
		}
		this.highlighted = [];
	}
}
window.onload = function(){
	console.log("hello");
	var manager = new Manager();
}