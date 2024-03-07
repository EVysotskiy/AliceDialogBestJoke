import re

def parse_num(numText):
    if type(numText) != str:
        return numText

    nums = re.findall(r'\d+', numText)
    return int(nums[0]) if len(nums) > 0 else numText
