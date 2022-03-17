import "./styles.css";
import { useEffect, useState } from "react";
import GameService from "../src/services/gameService";
import CheckBoxOutlineBlankTwoToneIcon from "@mui/icons-material/CheckBoxOutlineBlankTwoTone";
import DirectionsBoatFilledRoundedIcon from "@mui/icons-material/DirectionsBoatFilledRounded";
import DisabledByDefaultRoundedIcon from "@mui/icons-material/DisabledByDefaultRounded";

function App() {
  const [firstPlayer, setFirstPlayer] = useState<number[][]>([
    Array(10).fill(Array(10).fill(null)),
  ]);
  const [secondPlayer, setSecondPlayer] = useState<number[][]>([
    Array(10).fill(Array(10).fill(null)),
  ]);

  const [status, setStatus] = useState<string>("");

  const [player, setPlayer] = useState<number>(1);
  const [end, setEnd] = useState<boolean>(false);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  function RenderPlayer(board: number[][]) {
    return board.map((row) => {
      return (
        <div className="rowBoard">
          <div className="rowBoard">
            {row.map((column) => {
              if (column === 1 || column === 5 || column === 2)
                return (
                  <CheckBoxOutlineBlankTwoToneIcon className="icon"></CheckBoxOutlineBlankTwoToneIcon>
                );
              else if (column === 4)
                return (
                  <DirectionsBoatFilledRoundedIcon className="icon"></DirectionsBoatFilledRoundedIcon>
                );
              else
                return (
                  <DisabledByDefaultRoundedIcon className="icon"></DisabledByDefaultRoundedIcon>
                );
            })}
          </div>
        </div>
      );
    });
  }
  function simulate() {
    setInterval(() => {
      if (isLoading && !end) {
        var element = document.querySelector('button[id="play"]');
        if (element?.innerHTML === "Graj") simulateMouseClick(element);
      }
    }, 100);
  }

  const mouseClickEvents = ["mousedown", "click", "mouseup"];
  function simulateMouseClick(element: any) {
    mouseClickEvents.forEach((mouseEventType) =>
      element.dispatchEvent(
        new MouseEvent(mouseEventType, {
          view: window,
          bubbles: true,
          cancelable: true,
          buttons: 1,
        })
      )
    );
  }

  const play = async (player: number) => {
    await GameService.movement(player === 1 ? secondPlayer : firstPlayer).then(
      (result) => {
        if (player === 1) setSecondPlayer(result.data.board);
        else setFirstPlayer(result.data.board);

        if (result.data.statusGame === 1) {
          setStatus(
            "Trafiony. Gracz " + player.toString() + " wykonuje ponownie ruch."
          );
        } else if (result.data.statusGame === 2) {
          setStatus("Nietrafiony. Kolejny gracz.");
          setPlayer(player === 1 ? 2 : 1);
        } else {
          setStatus(
            "Koniec gry. Trafiony zatopiony. Wygrywa gracz: " +
              player.toString()
          );
          setEnd(true);
        }
      }
    );
  };

  const getData = async () => {
    var result = await GameService.getBoards();
    setFirstPlayer(result.data.firstPlayer);
    setSecondPlayer(result.data.secondPlayer);
  };
  useEffect(() => {
    if (!isLoading) getData();
    simulate();
  }, [isLoading, end]);

  return (
    <div className="App">
      <div className="text tittle">BattleShip Game</div>
      <div className="text">{status}</div>
      <div className="fplayer board">
        <p className="text">Gracz 1</p>
        {RenderPlayer(firstPlayer)}
      </div>
      <div className="splayer board">
        <p className="text">Gracz 2</p>
        {RenderPlayer(secondPlayer)}
      </div>
      <button
        id="play"
        className="button"
        onClick={async () => {
          if (!end) await play(player);
          else window.location.reload();
        }}
      >
        {end ? "Nowa gra" : "Graj"}
      </button>
      {!isLoading && (
        <button
          id="simulate"
          className="button"
          onClick={() => {
            setIsLoading(true);
          }}
        >
          Symulacja
        </button>
      )}
    </div>
  );
}

export default App;
