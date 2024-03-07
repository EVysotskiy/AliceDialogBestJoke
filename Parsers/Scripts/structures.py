import json
from parser_utils import parse_num

class Item:
    def __init__(self, text):
        self.text = text

    def to_dict(self):
        dict = {}
        dict["Text"] = self.text

        return dict

    def __str__(self):
        return json.dumps(self.to_dict(),ensure_ascii=False)

    def __repr__(self):
        return str(self)

class Joke(Item):
    def __init__(self, text):
        super().__init__(text)

    def to_dict(self):
        dict = super().to_dict()

        return dict

class Product(Item):
    def __init__(self, title, source, store, current_price, discount = 0, original_price = None, preview = "", platform = "PC",text_description = ""):
        super().__init__(title, source, store, preview,text_description)
        self.current_price = parse_num(current_price)
        self.discount = parse_num(discount)
        self.original_price = parse_num(original_price)
        self.platform = platform

    def to_dict(self):
        dict = super().to_dict()
        dict["CurrentPrice"] = self.current_price
        dict["Discount"] = self.discount
        dict["OriginalPrice"] = self.original_price
        dict["Platform"] = self.platform

        return dict

class News(Item):
    def __init__(self, title, source, store, preview = "", text = "", tags=[]):
        super().__init__(title, source, store, preview)
        self.text = text
        self.tags = tags

    def to_dict(self):
        dict = super().to_dict()
        dict["Text"] = self.text
        dict["Tags"] = self.tags

        return dict