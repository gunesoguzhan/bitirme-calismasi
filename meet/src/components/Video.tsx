function Video({src, muted, autoPlay}:{src:string, muted: boolean, autoPlay: boolean}) {
    return (
        <video src={src} muted={muted} autoPlay={autoPlay}/>
    )
}

export default Video