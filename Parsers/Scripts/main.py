import json

from parser_manager import ParserManager

CONFIG_PATH = "../config.json"

f = open(CONFIG_PATH)
config = json.load(f)

parserManager = ParserManager(config)
parserManager.run()