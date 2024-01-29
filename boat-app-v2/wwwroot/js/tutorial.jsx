class BoatBox extends React.Component {
    state = { data: this.props.initialData };
    loadBoatsFromServer = () => {
        const xhr = new XMLHttpRequest();
        xhr.open('get', this.props.url, true);
        xhr.onload = function() {
            var data = JSON.parse(xhr.responseText);
            this.setState({ data: data });
        }.bind(this);
        xhr.send();
    }

    handleBoatDelete = boat => {
        var xhr = new XMLHttpRequest();
        var data = new FormData();

        var boatObject = boat.boat;
        data.append('code', boatObject.code);

        data.append('name', boatObject.name);
        data.append('length', boatObject.length);
        data.append('width', boatObject.width);
        xhr.open('post', this.props.deleteUrl, true);
        xhr.onload = function() {
            this.loadBoatsFromServer();
        }.bind(this);
        xhr.send(data);
    }
    handleBoatSubmit = boat => {
        var boats = this.state.data;

        boat.code = "AZCD-1134-C2"
        var newBoats = boats.concat([boat]);
        this.setState({ data: newBoats });

        var data = new FormData();
        data.append('name', boat.name);
        data.append('length', boat.length);
        data.append('width', boat.width);

        var xhr = new XMLHttpRequest();
        xhr.open('post', this.props.submitUrl, true);
        xhr.onload = function() {
            this.loadBoatsFromServer();
        }.bind(this);
        xhr.send(data);
    }
    componentDidMount() {
        window.setInterval(this.loadBoatsFromServer, this.props.pollInterval);
    }
    render() {
        return (
            <div className="boatBox">
                <h1>Boats</h1>
                <BoatList loadBoatsFromServer = {this.loadBoatsFromServer} updateUrl = {this.props.updateUrl} onBoatDelete ={this.handleBoatDelete} data={this.state.data} />
                <BoatForm onBoatSubmit={this.handleBoatSubmit} />
            </div>
        );
    }
}

class Boat extends React.Component {
    render() {
        return (
            <div className="boat">
                <h2 className="boatName">{this.props.name}</h2>
                {this.props.children}
            </div>
        );
    }
}

class EditableText extends React.Component {
    constructor(props)
    {
        super(props);
        this.state = {
            text: this.props.initialText,
            type: this.props.textType,
            boat: this.props.boat,
            isEditing: false
        };
        
        this.handleDoubleClick = this.handleDoubleClick.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.handleBlur = this.handleBlur.bind(this);
        this.keypressHandler = this.keypressHandler.bind(this);
    }

    handleDoubleClick = () => {
        this.setState({ isEditing: true });
    };

    handleChange = (event) => {
        this.setState({ text: event.target.value });
    };

    validateInput() {
        if(this.props.initialText.trim() === this.state.text.trim()) return;
        if(this.state.text.trim().length === 0) return;
        if(this.state.type === "length" || this.state.type === "width")
            if(isNaN(+this.state.text)) return;
    }
    updateBoat() {
        if(this.state.type === "name") this.state.boat.name = this.state.text
        if(this.state.type === "length") this.state.boat.length = this.state.text
        if(this.state.type === "width") this.state.boat.width = this.state.text
        this.props.onBoatUpdate({boat: this.state.boat});
    }
    handleBlur = () => {
        this.validateInput()
        this.updateBoat()
        this.setState({ isEditing: false });
    };
    keypressHandler = event => {
        if (event.key === "Enter") {
            this.validateInput()
            this.updateBoat()
            this.setState({ isEditing: false });
        }
    };
    render() {
        return (
            <div onDoubleClick={this.handleDoubleClick}>
                {this.state.isEditing ? (
                    <input
                        type="text"
                        value={this.state.text}
                        onChange={this.handleChange}
                        onBlur={this.handleBlur}
                        onKeyDown={this.keypressHandler}
                    />
                ) : (
                    <span>{this.state.text}</span>
                )}
            </div>
        );
    }
}


class BoatList extends React.Component {
    constructor(props) {
        super(props);
        this.state = { removeBoat: '', updateBoat: '' };
        this.handleBoatUpdate = this.handleBoatUpdate.bind(this);
        this.handleRemove = this.handleRemove.bind(this);
    };
    
    handleBoatUpdate(boat) {
        var xhr = new XMLHttpRequest();
        var data = new FormData();

        var boatObject = boat.boat;
        data.append('code', boatObject.code);
        data.append('name', boatObject.name);
        data.append('length', boatObject.length);
        data.append('width', boatObject.width);
        
        xhr.open('post', this.props.updateUrl, true);
        xhr.onload = function() {
            this.props.loadBoatsFromServer();
        }.bind(this);
        xhr.send(data);
    }
    handleRemove(boat) {
        this.state.removeBoat = boat
        this.props.onBoatDelete({ boat: this.state.removeBoat});
    }
    render() {
        const boatNodes = this.props.data.map(boat => (
            <Boat key={boat.code}>
                <EditableText boat={boat} initialText={boat.name} textType="name" onBoatUpdate ={this.handleBoatUpdate} />
                <EditableText boat={boat} initialText={boat.length} textType="length" onBoatUpdate ={this.handleBoatUpdate} />
                <EditableText boat={boat} initialText={boat.width} textType="width" onBoatUpdate ={this.handleBoatUpdate} />
                <button type="button"  onClick={() => this.handleRemove(boat)}>
                    Delete
                </button>
            </Boat>
        ));
        return <div className="boatList">{boatNodes}</div>;
    }
}

class BoatForm extends React.Component {
   constructor(props) {
        super(props);
        this.state = { name: '', length: '', width: ''  };
        this.handleNameChange = this.handleNameChange.bind(this);
        this.handleLengthChange = this.handleLengthChange.bind(this);
        this.handleWidthChange = this.handleWidthChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }
    handleNameChange(e) {
        this.setState({ name: e.target.value });
    }
    handleLengthChange(e) {
        this.setState({ length: e.target.value });
    }
    handleWidthChange(e) {
        this.setState({ width: e.target.value });
    }
    handleSubmit = e => {
        e.preventDefault();
        const name = this.state.name.trim();
        const length = this.state.length.trim();
        const width = this.state.width.trim();
        if (!name || !length || !width) {
            return;
        }
        this.props.onBoatSubmit({ name: name, length: length, width: width });
        this.setState({ name: '', length: '', width: '' });
    }
    render() {
        return (
            <form className="boatForm" onSubmit={this.handleSubmit}>
                <input type="text" placeholder="Boat name"
                       value={this.state.name}
                       onChange={this.handleNameChange} />
                <input type="text" placeholder="length"
                       value={this.state.length}
                       onChange={this.handleLengthChange} />
                <input type="text" placeholder="width"
                       value={this.state.width}
                       onChange={this.handleWidthChange}/>
                <input type="submit" value="Post" />
            </form>
        );
    }
}


