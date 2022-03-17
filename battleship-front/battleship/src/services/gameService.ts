import http from "../http-common";
import { StartBattleship } from "../models/start_battleship";
import { OutputBattleship } from "../models/output_battleship";

class GameService {
  async getBoards() {
    return await http.get<StartBattleship>("/Battleship/playersBoards");
  }

  async movement(board: number[][]) {
    return await http.post<OutputBattleship>("/Battleship/movement", board);
  }
}
export default new GameService();
