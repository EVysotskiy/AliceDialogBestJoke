import os

from anecdotru_parser import AnecdotruParser
from anecdotica_parser import AnecdoticaParser

OUTPUT_PATH = "../Output/"

PARSERS_DICT = {
    "anecdotica": AnecdoticaParser(),
    "anecdotru": AnecdotruParser(),
}

class ParserManager:
    def __init__(self, config):
        self.__config = config
        
        for parserKey in self.__config["ActiveParsers"]:
            if parserKey not in PARSERS_DICT:
                raise Exception("Config contains unrecognized parser key \"" + parserKey + "\"!")


    def __save_result(self, result, fileName):
        os.makedirs(OUTPUT_PATH, exist_ok=True)
        filename = OUTPUT_PATH + fileName
        
        with open(filename, 'w',encoding='utf-8') as f:
            f.write(str(result))

    def run(self):
        completeCatalog = []
        for parserKey in self.__config["ActiveParsers"]:
            parser = PARSERS_DICT[parserKey]
            
            catalog = parser.get_catalog()
            completeCatalog.extend(catalog)

        self.__save_result(completeCatalog, 'catalog.json')

            




