from bs4 import Tag, NavigableString
from base_parser import BaseParser
from structures import Joke

class AnecdotruParser(BaseParser):
    def __init__(self):
        links = []
        for number in range(1,12):
            links.append("https://www.anekdot.ru/release/anekdot/year/2023/{arg1}".format(arg1=number))

        super().__init__()
        self.root = "https://www.anekdot.ru"
        self.links = links
        self.__additional_links = None

    def _parse_soup(self, soup):
        jokes = []
        joke_blocks = soup.find_all('div', 'text')

        for block in joke_blocks:
            joke_text = ""
            for item in block.contents:
                if type(item) is NavigableString:
                    joke_text += item.replace('\n●', ' ').replace('♦', '*').replace('©', "Цитата: ").replace(' ',' ')

            my_bytes = joke_text.encode()
            s = my_bytes.decode('utf-8')
            p = Joke(s)
            jokes.append(p)

        return jokes