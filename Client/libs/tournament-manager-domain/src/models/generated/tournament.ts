///////////////////////////////////////////////////////////
// This file is generated by TournamentManager.Tools.Cli //
///////////////////////////////////////////////////////////

import { BaseEntity } from "./base-entity";
import { Sport } from "./../../enums/generated/sport";

export interface Tournament extends BaseEntity{
    name: string
    sport: Sport
}
