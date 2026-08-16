import typing
from dataclasses import dataclass
from Options import Option, Range, Choice, Toggle, PerGameCommonOptions, OptionDict

class RandomizeGems(Toggle):
    """If the gem unlocks should be randomized"""
    display_name = "Gems"
    option_true = 1
    option_false = 0
    default = 1

class GemsInPool(Range):
    """How many Gems are in the pool"""
    display_name = "Gems in Pool"
    range_start = 0
    range_end = 40
    default = 4

class GemsRequired(Range):
    """What percentage of available gems (rounded down) is required to open the door"""
    display_name = "Gems Required"
    range_start = 0
    range_end = 100
    default = 75

class RequiredEndings(Range):
    """How many endings are required to be completed to win the game."""
    display_name = "Endings"
    range_start = 0
    range_end = 99
    default = 40

class RequireFailableJumps(Toggle):
    """This includes jumps in logic that are difficult and result in death if missed"""
    display_name = "RequireFailableJumps"
    option_true = 1
    option_false = 0
    default = 0

class RequireHardCombat(Toggle):
    """This adds ending 49 into logic without shield"""
    display_name = "RequireHardCombat"
    option_true = 1
    option_false = 0
    default = 0

class AddTreasureSword(Toggle):
    """This adds the sword in the treasure room into the sword progression"""
    display_name = "AddTreasureSword"
    option_true = 1
    option_false = 0
    default = 0

class NonStopMode(Choice):
    """This turns on non-stop mode, which allows continuing after getting certain endings.
    If set to canonical, it will only allow continuing after endings that "make sense". E.g. endings where Lonk does not die canonically.
    If set to logical, it will allow continuing after any ending that does not break logic.
    If set to everything, it will allow continuing after any ending. This will allow you to break logic because you can e.g. walk in lava.
    """
    option_off = 0
    option_canonical = 1
    option_logical = 2
    option_everything = 3
    display_name = "NonStopMode"
    default = 2

class UseExperimentalRegionGraph(Toggle):
    """This turns on the experimental region graph. This options is still bugged and likely to slow down generation or produce unbeatable seeds. Turn on at your own risk."""
    display_name = "UseExperimentalRegionGraph"
    option_true = 1
    option_false = 0
    default = 0

class LogicDict(OptionDict):
    display_name = "Logic"

@dataclass
class ReventureOptions(PerGameCommonOptions):
    endings: RequiredEndings
    randomizeGems: RandomizeGems
    gemsInPool: GemsInPool
    gemsRequired: GemsRequired
    hardjumps: RequireFailableJumps
    hardcombat: RequireHardCombat
    treasureSword: AddTreasureSword
    nonStopMode: NonStopMode
    experimentalRegionGraph: UseExperimentalRegionGraph
    logic: LogicDict